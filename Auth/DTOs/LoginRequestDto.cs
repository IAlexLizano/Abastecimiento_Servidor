namespace Auth.DTOs
{
    /// <summary>
    /// Solicitud de inicio de sesión
    /// </summary>
    public class LoginRequestDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
