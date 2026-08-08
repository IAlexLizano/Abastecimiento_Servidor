namespace Domains.Entities;

public partial class RoleMenu
{
    public int IdMenu { get; set; }

    public int IdRole { get; set; }

    public bool? IsActive { get; set; }

    public virtual Menu IdMenuNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
