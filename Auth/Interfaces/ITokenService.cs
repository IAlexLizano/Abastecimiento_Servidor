using System.Security.Claims;

namespace Auth.Interfaces
{
    /// <summary>
    /// Interfaz para generación y validación de tokens JWT
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Genera un token JWT con los claims proporcionados
        /// </summary>
        string GenerateToken(List<Claim> claims, int expirationMinutes = 60);

        /// <summary>
        /// Extrae los claims de un token JWT
        /// </summary>
        IEnumerable<Claim> GetClaimsFromToken(string token);
    }
}
