namespace Domains.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<AccessControl> AccessControl { get; set; } = new List<AccessControl>();

    public virtual ICollection<RoleMenu> RoleMenu { get; set; } = new List<RoleMenu>();

    public virtual ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
}
