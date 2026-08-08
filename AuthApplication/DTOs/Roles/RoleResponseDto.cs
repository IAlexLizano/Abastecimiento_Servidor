namespace AuthApplication.DTOs.Roles
{
    /// <summary>
    /// DTO para la respuesta de un rol
    /// </summary>
    public class RoleResponseDto
    {
        public int IdRole { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
