namespace Domains.Entities;

public partial class AccessControl
{
    public int IdAccess { get; set; }

    public int IdRole { get; set; }

    public int IdService { get; set; }

    public bool IsActive { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual Service IdServiceNavigation { get; set; } = null!;
}
