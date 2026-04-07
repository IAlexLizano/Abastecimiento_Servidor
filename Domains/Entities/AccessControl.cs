namespace Domains.Entities;

public partial class AccessControl
{
    public int AccessId { get; set; }

    public int RoleId { get; set; }

    public int ServiceId { get; set; }

    public bool IsActive { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
