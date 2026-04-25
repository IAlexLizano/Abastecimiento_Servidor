namespace AuthApplication.DTOs
{
    /// <summary>
    /// Solicitud de autenticación del usuario
    /// </summary>
    public class AuthenticationRequest
    {
        public string User { get; set; } = null!;
        public string? Rol { get; set; }
    }
}
