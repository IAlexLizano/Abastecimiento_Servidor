namespace Domains.Entities;

public partial class UserRole
{
    public int RoleId { get; set; }

    public int UserId { get; set; }

    public DateOnly? AssignedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;
}
