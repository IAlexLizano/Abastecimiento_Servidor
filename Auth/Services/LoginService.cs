using Auth.Context;
using Auth.Interfaces;
using AuthApplication.DTOs;
using AuthApplication.Interfaces;
using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Application.Exceptions;
using Shared.Application.Wrappers;
using Shared.Global;

namespace Auth.Services
{
    /// <summary>
    /// Servicio para manejo de login de usuarios
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly ApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly IHashingService _hashingService;
        //private readonly IGPGService _GPGService;
        private readonly InformationSession _global;

        public LoginService(
            ApplicationContext dbContext,
            IAccountService accountService,
            IHashingService hashingService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }

        public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException();

            var usuario = await _dbContext.Set<UserAccount>()
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower());

            if (usuario == null)
                throw new ApiException("Usuario o contraseña incorrectos.");

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
                .Where(ur => ur.IsActive == true && ur.Role.IsActive)
                .OrderByDescending(ur => ur.AssignedAt)
                .FirstOrDefault();

            // Generar token mediante AccountService
            var authRequest = new AuthenticationRequest
            {
                User = usuario.Username,
                Rol = rolPrincipal?.Role.Name
            };

            var authResponse = await _accountService.AuthenticateAsync(authRequest);

            var response = new LoginResponseDto
            {
                UserId = usuario.UserId,
                Username = usuario.Username,
                FullName = $"{usuario.FirstName} {usuario.LastName}",
                RoleId = rolPrincipal?.RoleId,
                RoleName = rolPrincipal?.Role.Name,
                Token = authResponse.Data.JWToken
            };

            return new Response<LoginResponseDto>(response, "Login exitoso");
        }

        public async Task<Response<List<MenuItemResponseDto>>> GetMenuByUserAsync()
        {
            var usuario = await _dbContext.Set<UserAccount>()
                .AsNoTracking()
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RoleMenu)
                            .ThenInclude(rm => rm.Menu)
                .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(_global.UserName.ToLower())) 
                ?? throw new ApiException("Usuario no encontrado");
            
            if (!usuario.IsActive)
                throw new ApiException("El usuario se encuentra inactivo");

            // Obtener roles activos
            var rolesActivos = usuario.UserRole
                .Where(ur => ur.IsActive == true && ur.Role.IsActive)
                .Select(ur => ur.Role)
                .Distinct()
                .ToList();

            if (!rolesActivos.Any())
                throw new ApiException("El usuario no tiene roles asignados");

            // Obtener menús por roles
            var menus = rolesActivos
                .SelectMany(r => r.RoleMenu)
                .Where(rm => rm.Menu.IsActive == true && rm.Menu.IsVisible == true)
                .Select(rm => rm.Menu)
                .Distinct()
                .ToList();

            // Construir jerarquía de menús
            var menusRaiz = menus
                .Where(m => m.ParentMenuId == null)
                .OrderBy(m => m.DisplayOrder)
                .ToList();

            var respuesta = menusRaiz.Select(m => BuildMenuHierarchy(m, menus)).ToList();

            return new Response<List<MenuItemResponseDto>>(respuesta, "Menús obtenidos correctamente");
        }

        /// <summary>
        /// Construye la jerarquía de menú de forma recursiva
        /// </summary>
        private MenuItemResponseDto BuildMenuHierarchy(Menu menu, List<Menu> allMenus)
        {
            var submenus = allMenus
                .Where(m => m.ParentMenuId == menu.MenuId)
                .OrderBy(m => m.DisplayOrder)
                .ToList();

            return new MenuItemResponseDto
            {
                MenuId = menu.MenuId,
                Name = menu.Name,
                Url = menu.Url,
                Icon = menu.Icon,
                DisplayOrder = menu.DisplayOrder,
                SubMenu = submenus.Select(sm => BuildMenuHierarchy(sm, allMenus)).ToList()
            };
        }
    }
}
