using System.Security.Claims;

namespace Auth.Helpers
{
    /// <summary>
    /// Helper para manejo de claims y tokens
    /// </summary>
    public static class ClaimHelper
    {
        /// <summary>
        /// Obtiene un claim específico de la colección
        /// </summary>
        public static string? GetClaimValue(IEnumerable<Claim> claims, string claimType)
        {
            return claims?.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

        /// <summary>
        /// Obtiene el ID del usuario desde los claims
        /// </summary>
        public static int? GetUserId(IEnumerable<Claim> claims)
        {
            var userIdClaim = GetClaimValue(claims, ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        /// <summary>
        /// Obtiene el nombre de usuario desde los claims
        /// </summary>
        public static string? GetUsername(IEnumerable<Claim> claims)
        {
            return GetClaimValue(claims, ClaimTypes.Name);
        }

        /// <summary>
        /// Obtiene todos los roles del usuario
        /// </summary>
        public static List<string> GetRoles(IEnumerable<Claim> claims)
        {
            return claims?
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
        }

        /// <summary>
        /// Verifica si el usuario tiene un rol específico
        /// </summary>
        public static bool HasRole(IEnumerable<Claim> claims, string roleName)
        {
            return GetRoles(claims).Any(r => r.Contains(roleName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
