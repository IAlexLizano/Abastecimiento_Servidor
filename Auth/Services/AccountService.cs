using Auth.Context;
using Auth.DTOs;
using Auth.Exceptions;
using Auth.Interfaces;
using Auth.Wrappers;
using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Auth.Services
{
    /// <summary>
    /// Servicio para autenticación y generación de tokens
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly ApplicationContext _dbContext;
        private readonly ITokenService _tokenService;

        public AccountService(
            ApplicationContext dbContext,
            ITokenService tokenService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.User))
                throw new ValidationException("El nombre de usuario es requerido");

            var usuario = await _dbContext.Set<UserAccount>()
                .AsNoTracking()
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == request.User.ToLower());

            if (usuario == null)
                throw new ApiException("Usuario o contraseña incorrectos.");

            if (!usuario.IsActive)
                throw new ApiException("El usuario se encuentra inactivo.");

            // Obtener roles activos del usuario
            var roles = usuario.UserRole
                .Where(ur => ur.IsActive == true && ur.Role.IsActive)
                .OrderByDescending(ur => ur.AssignedAt)
                .ToList();

            if (!roles.Any())
                throw new ApiException("El usuario no tiene roles asignados.");

            // Generar claims para el token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UserId.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty),
                new Claim("FullName", $"{usuario.FirstName} {usuario.LastName}"),
                new Claim("IdCard", usuario.IdCard),
                new Claim("CreatedAt", usuario.CreatedAt?.ToString("yyyy-MM-dd") ?? string.Empty)
            };

            // Agregar roles como claims
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, $"{rol.RoleId}-{rol.Role.Name}"));
            }

            // Generar token
            var token = _tokenService.GenerateToken(claims, expirationMinutes: 60);

            var response = new AuthenticationResponse
            {
                Id = usuario.UserId,
                UserName = usuario.Username,
                Email = usuario.Email ?? string.Empty,
                JWToken = token
            };

            return new Response<AuthenticationResponse>(response, $"Usuario {usuario.Username} autenticado correctamente");
        }
    }
}
