namespace Domains.Entities;

public partial class Menu
{
    public int MenuId { get; set; }

    public string Name { get; set; } = null!;

    public bool? HasSubmenu { get; set; }

    public string Url { get; set; } = null!;

    public int? ParentMenuId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsVisible { get; set; }

    public int? DisplayOrder { get; set; }

    public string? Icon { get; set; }

    public virtual ICollection<Menu> InverseParentMenu { get; set; } = new List<Menu>();

    public virtual Menu? ParentMenu { get; set; }

    public virtual ICollection<RoleMenu> RoleMenu { get; set; } = new List<RoleMenu>();
}
