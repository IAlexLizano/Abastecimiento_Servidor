namespace AuthApplication.DTOs.Users
{
    /// <summary>
    /// Respuesta con información del usuario
    /// </summary>
    public class UserResponseDto
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Dni { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public UserRoleDto Role { get; set; }

        public bool IsActive { get; set; }

        public DateOnly? CreatedAt { get; set; }
    }

    public class UserRoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
