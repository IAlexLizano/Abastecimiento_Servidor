namespace AuthApplication.DTOs.Users
{
    /// <summary>
    /// Solicitud para actualizar información de un usuario
    /// </summary>
    public class UpdateUserRequestDto
    {
        public string UserName { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Dni { get; set; }
        public int RoleId { get; set; }
    }
}
