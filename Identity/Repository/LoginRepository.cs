using Identity.Context;
using AuthApplication.DTOs;
using AuthApplication.Interfaces;
using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Wrappers;
using Shared.Global;
using Microsoft.Extensions.Configuration;

namespace Identity.Repository
{
    /// <summary>
    /// Servicio para manejo de login de usuarios
    /// </summary>
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountService;
        private readonly IHashingRepository _hashingService;
        //private readonly IGPGService _GPGService;
        private readonly InformationSession _global;

        public LoginRepository(
            ApplicationContext dbContext,
            IAccountRepository accountService,
            IHashingRepository hashingService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                var usuario = await _dbContext.RegisteredUser
    .Include(u => u.UserRole)
        .ThenInclude(ur => ur.IdRoleNavigation)
    .FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower()) ?? throw new ApiException("Usuario o contraseña incorrectos.");
                
                if (!usuario.IsActive)
                    throw new ApiException("El usuario se encuentra inactivo. Contacte al administrador.");

                // Verificar contraseña
                if (string.IsNullOrWhiteSpace(usuario.PasswordHash) || !_hashingService.VerifyPassword(request.Password, usuario.PasswordHash))
                {
                    // Incrementar intentos fallidos
                    usuario.LoginAttempts = (usuario.LoginAttempts ?? 0) + 1;

                    // Bloquear usuario después de 3 intentos fallidos
                    if (usuario.LoginAttempts >= 3)
                    {
                        usuario.IsActive = false;
                    }

                    await _dbContext.SaveChangesAsync();
                    throw new ApiException("Usuario o contraseña incorrectos.");
                }

                // Resetear intentos fallidos
                if (usuario.LoginAttempts.HasValue && usuario.LoginAttempts > 0)
                {
                    usuario.LoginAttempts = 0;
                    await _dbContext.SaveChangesAsync();
                }

                // Obtener rol principal
                var rolPrincipal = usuario.UserRole
                    .Where(ur => ur.IsActive == true && ur.IdRoleNavigation.IsActive)
                    .OrderByDescending(ur => ur.AssignedAt)
                    .FirstOrDefault();

                // Generar token mediante AccountService
                var authRequest = new AuthenticationRequest
                {
                    User = usuario.Username,
                    Rol = rolPrincipal?.IdRoleNavigation.Name
                };

                var authResponse = await _accountService.AuthenticateAsync(authRequest);
                
                return new LoginResponseDto
                {
                    UserId = usuario.IdUser,
                    Username = usuario.Username,
                    FullName = $"{usuario.FirstName} {usuario.LastName}",
                    RoleId = rolPrincipal?.IdRole,
                    RoleName = rolPrincipal?.IdRoleNavigation.Name,
                    Token = authResponse.Data.JWToken
                };
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al ingresar al sistema {ex}");
            }
        }

        public async Task<List<MenuItemResponseDto>> GetMenuByUserAsync()
        {
            try
            {
                var usuario = await _dbContext.RegisteredUser
    .AsNoTracking()
    .Include(u => u.UserRole)
        .ThenInclude(ur => ur.IdRoleNavigation)
            .ThenInclude(r => r.RoleMenu)
                .ThenInclude(rm => rm.IdMenuNavigation)
    .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(_global.UserName.ToLower()))
    ?? throw new ApiException("Usuario no encontrado");

                if (!usuario.IsActive)
                    throw new ApiException("El usuario se encuentra inactivo");

                // Obtener roles activos
                var rolesActivos = usuario.UserRole
                    .Where(ur => ur.IsActive == true && ur.IdRoleNavigation.IsActive)
                    .Select(ur => ur.IdRoleNavigation)
                    .Distinct()
                    .ToList();

                if (rolesActivos.Count == 0)
                    throw new ApiException("El usuario no tiene roles asignados");

                // Obtener menús por roles, ordenados y sin jerarquía
                var menus = rolesActivos
                    .SelectMany(r => r.RoleMenu)
                    .Where(rm => rm.IdMenuNavigation.IsActive == true && rm.IdMenuNavigation.IsVisible == true)
                    .Select(rm => rm.IdMenuNavigation)
                    .Distinct()
                    .OrderBy(m => m.DisplayOrder)
                    .ToList();

                // Convertir a DTOs
                var respuesta = menus.Select(m => new MenuItemResponseDto
                {
                    MenuId = m.IdMenu,
                    Name = m.Name,
                    Icon = m.Icon,
                    Url = m.Url,
                    DisplayOrder = m.DisplayOrder,
                    SubMenu = new List<MenuItemResponseDto>()
                }).ToList();

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error al obtener el menú {ex}");
            }
        }
    }
}
