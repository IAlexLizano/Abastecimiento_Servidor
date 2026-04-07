namespace Domains.Entities;

public partial class RoleMenu
{
    public int MenuId { get; set; }

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Menu Menu { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
