namespace Domains.Entities;

public partial class Menu
{
    public int IdMenu { get; set; }

    public string Name { get; set; } = null!;

    public string Url { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool? IsVisible { get; set; }

    public int? DisplayOrder { get; set; }

    public string? Icon { get; set; }

    public virtual ICollection<RoleMenu> RoleMenu { get; set; } = new List<RoleMenu>();
}
