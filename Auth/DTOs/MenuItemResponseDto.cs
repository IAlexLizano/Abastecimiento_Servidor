namespace Auth.DTOs
{
    /// <summary>
    /// Elemento de menú en la respuesta
    /// </summary>
    public class MenuItemResponseDto
    {
        public int MenuId { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? Icon { get; set; }
        public int? DisplayOrder { get; set; }
        public List<MenuItemResponseDto> SubMenu { get; set; } = new();
    }
}
