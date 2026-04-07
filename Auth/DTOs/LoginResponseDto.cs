namespace Auth.DTOs
{
    /// <summary>
    /// Respuesta de inicio de sesión
    /// </summary>
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string Token { get; set; } = null!;
    }
}
