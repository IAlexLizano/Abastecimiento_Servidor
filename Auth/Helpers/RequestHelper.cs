using Microsoft.AspNetCore.Http;

namespace Auth.Helpers
{
    /// <summary>
    /// Helper para obtener información de la solicitud HTTP
    /// </summary>
    public static class RequestHelper
    {
        /// <summary>
        /// Obtiene la dirección IP del cliente
        /// </summary>
        public static string GetClientIpAddress(HttpContext context)
        {
            if (context == null)
                return "unknown";

            try
            {
                // Intentar obtener la IP del header X-Forwarded-For (para proxies)
                if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(forwardedFor))
                    {
                        var ipAddress = forwardedFor.Split(',').FirstOrDefault()?.Trim();
                        if (!string.IsNullOrEmpty(ipAddress))
                            return ipAddress;
                    }
                }

                // Obtener la IP de la conexión remota
                return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        /// <summary>
        /// Obtiene el user agent del navegador
        /// </summary>
        public static string GetUserAgent(HttpContext context)
        {
            if (context == null || !context.Request.Headers.ContainsKey("User-Agent"))
                return "unknown";

            return context.Request.Headers["User-Agent"].ToString();
        }
    }
}
