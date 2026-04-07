namespace Domains.Entities;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string IdCard { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public string? PasswordHash { get; set; }

    public int? LoginAttempts { get; set; }

    public virtual ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
}
