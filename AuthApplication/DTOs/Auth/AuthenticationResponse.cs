namespace AuthApplication.DTOs
{
    /// <summary>
    /// Respuesta de autenticación con token JWT
    /// </summary>
    public class AuthenticationResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string JWToken { get; set; } = null!;
        public string? RefreshToken { get; set; }
    }
}
