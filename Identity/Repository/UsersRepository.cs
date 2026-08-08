using Identity.Context;
using AuthApplication.Interfaces;
using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Wrappers;
using AuthApplication.DTOs.Users;

namespace Identity.Repository
{
    /// <summary>
    /// Servicio para gestión de usuarios
    /// La contraseña inicial se genera automáticamente con la cédula del usuario
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly IHashingRepository _hashingService;

        public UsersRepository(
            ApplicationContext dbContext,
            IHashingRepository hashingService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }

        public async Task<string> CreateUserAsync(CreateUserRequestDto request)
        {
            if (request == null)
                throw new ValidationException();

            // Verificar si el usuario ya existe por nombre de usuario
            if (await UserExistsByUsernameAsync(request.Username))
                throw new ApiException($"El nombre de usuario '{request.Username}' ya existe.");

            // Verificar si la cédula ya existe
            if (await UserExistsByIdCardAsync(request.IdCard))
                throw new ApiException($"Ya existe un usuario con la cédula '{request.IdCard}'.");

            if (!ExistsRole(request.RoleId))
                throw new ApiException($"El rol seleccionado no existe.");

            // La contraseña es igual a la cédula y se hashea
            var hashedPassword = _hashingService.HashPassword(request.IdCard);

            var nuevoUsuario = new RegisteredUser
            {
                Username = request.Username.Trim().ToUpper(),
                FirstName = request.FirstName.Trim().ToUpper(),
                LastName = request.LastName.Trim().ToUpper(),
                IdCard = request.IdCard.Trim(),
                Phone = request.Phone?.Trim(),
                Email = request.Email?.Trim(),
                PasswordHash = hashedPassword,
                IsActive = true,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                LoginAttempts = 0
            };

            try
            {
                _dbContext.RegisteredUser.Add(nuevoUsuario);
                _dbContext.UserRole.Add(new UserRole
                {
                    IdUserNavigation = nuevoUsuario,
                    IdRole = request.RoleId
                });
                await _dbContext.SaveChangesAsync();


                return "Usuario creado exitosamente";
            }
            catch (DbUpdateException ex)
            {
                throw new ApiException($"Error al crear el usuario: {ex.Message}");
            }
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {
            try
            {
                var usuario = await (from u in _dbContext.RegisteredUser
                                     join ur in _dbContext.UserRole on u.IdUser equals ur.IdUser
                                     join r in _dbContext.Role on ur.IdRole equals r.IdRole
                                     where u.IdUser == userId
                                     select new UserResponseDto
                                     {
                                         UserId = u.IdUser,
                                         Username = u.Username,
                                         FirstName = u.FirstName,
                                         LastName = u.LastName,
                                         Dni = u.IdCard,
                                         Phone = u.Phone,
                                         Email = u.Email,
                                         Role = new UserRoleDto
                                         {
                                             RoleId = r.IdRole,
                                             RoleName = r.Name
                                         },
                                         IsActive = u.IsActive,
                                         CreatedAt = u.CreatedAt
                                     }).FirstOrDefaultAsync();

                return usuario ?? throw new ApiException("Usuario no encontrado.");
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al obtener el usuario: {ex.Message}");
            }
        }

        public async Task<UserResponseDto> GetUserByUsernameAsync(string username)
        {
            try
            {
                var usuario = await (from u in _dbContext.RegisteredUser
                                     join ur in _dbContext.UserRole on u.IdUser equals ur.IdUser
                                     join r in _dbContext.Role on ur.IdRole equals r.IdRole
                                     where u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                     select new UserResponseDto
                                     {
                                         UserId = u.IdUser,
                                         Username = u.Username,
                                         FirstName = u.FirstName,
                                         LastName = u.LastName,
                                         Dni = u.IdCard,
                                         Phone = u.Phone,
                                         Email = u.Email,
                                         Role = new UserRoleDto
                                         {
                                             RoleId = r.IdRole,
                                             RoleName = r.Name
                                         },
                                         IsActive = u.IsActive,
                                         CreatedAt = u.CreatedAt
                                     }).FirstOrDefaultAsync();

                return usuario ?? throw new ApiException($"Usuario {username} no encontrado.");
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al obtener el usuario {ex}");
            }
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            try
            {
                var usuarios = await (from u in _dbContext.RegisteredUser
                                      join ur in _dbContext.UserRole.Where(x => x.IsActive == true) on u.IdUser equals ur.IdUser
                                      join r in _dbContext.Role on ur.IdRole equals r.IdRole
                                      //where u.IsActive == true
                                      orderby u.LastName
                                      select new UserResponseDto
                                      {
                                          UserId = u.IdUser,
                                          Username = u.Username,
                                          FirstName = u.FirstName,
                                          LastName = u.LastName,
                                          Dni = u.IdCard,
                                          Phone = u.Phone,
                                          Email = u.Email,
                                          Role = new UserRoleDto
                                          {
                                              RoleId = r.IdRole,
                                              RoleName = r.Name
                                          },
                                          IsActive = u.IsActive,
                                          CreatedAt = u.CreatedAt
                                      }).ToListAsync();

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al cargar usuarios {ex}");
            }
        }

        public async Task<string> UpdateUserAsync(int userId, UpdateUserRequestDto request)
        {
            try
            {
                var usuario = await _dbContext.RegisteredUser
                    .FirstOrDefaultAsync(u => u.IdUser == userId) ?? throw new ApiException("Usuario no encontrado.");

                if (!ExistsRole(request.RoleId))
                    throw new ApiException("Rol no encontrado.");

                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                usuario.FirstName = request.FirstName.Trim().ToUpper();
                usuario.LastName = request.LastName.Trim().ToUpper();
                usuario.Username = request.UserName.Trim().ToUpper();
                usuario.Email = request.Email?.Trim();
                usuario.Phone = request.Phone;
                await UpdateUserRole(userId, request.RoleId);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Usuario actualizado exitosamente";
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al actualizar el usuario: {ex.Message}");
            }
        }

        public async Task<string> DeactivateUserAsync(int userId)
        {
            try
            {
                var usuario = await _dbContext.RegisteredUser
                    .FirstOrDefaultAsync(u => u.IdUser == userId) ?? throw new ApiException("Usuario no encontrado.");
                
                if (!usuario.IsActive)
                    throw new ApiException("El usuario ya está desactivado.");

                usuario.IsActive = false;
                
                _dbContext.RegisteredUser.Update(usuario);
                await _dbContext.SaveChangesAsync();

                return $"Usuario {usuario.Username} desactivado exitosamente";
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al desactivar el usuario: {ex.Message}");
            }
        }

        public async Task<string> ReactivateUserAsync(int userId)
        {
            try
            {
                var usuario = await _dbContext.RegisteredUser
    .FirstOrDefaultAsync(u => u.IdUser == userId) ?? throw new ApiException("Usuario no encontrado.");
                
                if (usuario.IsActive)
                    throw new ApiException("El usuario ya está activo.");

                usuario.IsActive = true;
                usuario.LoginAttempts = 0; // Resetear intentos de login

                _dbContext.RegisteredUser.Update(usuario);
                await _dbContext.SaveChangesAsync();

                return $"Usuario {usuario.Username} reactivado exitosamente";
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al reactivar el usuario: {ex.Message}");
            }
        }

        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            return await _dbContext.RegisteredUser
                .AsNoTracking()
                .AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> UserExistsByIdCardAsync(string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
                return false;

            return await _dbContext.RegisteredUser
                .AsNoTracking()
                .AnyAsync(u => u.IdCard == idCard);
        }

        public async Task<List<UserResponseDto>> GetUsersByRoleIdAsync(int roleId)
        {
            try
            {
                var usuarios = await (from u in _dbContext.RegisteredUser
                                      join ur in _dbContext.UserRole.Where(x => x.IsActive == true) on u.IdUser equals ur.IdUser
                                      join r in _dbContext.Role on ur.IdRole equals r.IdRole
                                      where ur.IdRole == roleId && u.IsActive == true
                                      orderby u.LastName
                                      select new UserResponseDto
                                      {
                                          UserId = u.IdUser,
                                          Username = u.Username,
                                          FirstName = u.FirstName,
                                          LastName = u.LastName,
                                          Dni = u.IdCard,
                                          Phone = u.Phone,
                                          Email = u.Email,
                                          Role = new UserRoleDto
                                          {
                                              RoleId = r.IdRole,
                                              RoleName = r.Name
                                          },
                                          IsActive = u.IsActive,
                                          CreatedAt = u.CreatedAt
                                      }).ToListAsync();

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al obtener usuarios {ex}");
            }
        }

        private bool ExistsRole(int roleId)
        {
            return _dbContext.Role.Any(r => r.IdRole == roleId);
        }

        private async Task UpdateUserRole(int userId, int roleId)
        {
            var userRole = await _dbContext.UserRole.Where(ur => ur.IdUser == userId).ToListAsync();

            foreach (var ur in userRole)
            {
                if (ur.IdRole == roleId)
                {
                    if (ur.IsActive == true)
                        return;
                    ur.IsActive = true;
                    await _dbContext.SaveChangesAsync();
                    return;
                }
                ur.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }
            _dbContext.UserRole.Add(new UserRole
            {
                IdUser = userId,
                IdRole = roleId,
                AssignedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                IsActive = true
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
