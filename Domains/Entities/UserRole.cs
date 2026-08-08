namespace Domains.Entities;

public partial class UserRole
{
    public int IdRole { get; set; }

    public int IdUser { get; set; }

    public DateOnly? AssignedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual RegisteredUser IdUserNavigation { get; set; } = null!;
}
