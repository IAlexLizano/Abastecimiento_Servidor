namespace AuthApplication.DTOs.Users
{
    /// <summary>
    /// Solicitud para crear un nuevo usuario
    /// La contraseña se genera automáticamente con la cédula del usuario
    /// </summary>
    public class CreateUserRequestDto
    {
        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string IdCard { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public int RoleId { get; set; }
    }
}
