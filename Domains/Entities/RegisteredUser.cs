namespace Domains.Entities;

public partial class RegisteredUser
{
    public int IdUser { get; set; }

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

    public int? IdStation { get; set; }

    public virtual ICollection<BoxInspectionTeamMember> BoxInspectionTeamMember { get; set; } = new List<BoxInspectionTeamMember>();

    public virtual ICollection<Cardboard> Cardboard { get; set; } = new List<Cardboard>();

    public virtual ICollection<EngineeringIssue> EngineeringIssueIdUserReportsNavigation { get; set; } = new List<EngineeringIssue>();

    public virtual ICollection<EngineeringIssue> EngineeringIssueIdUserSolvesNavigation { get; set; } = new List<EngineeringIssue>();

    public virtual WorkStation? IdStationNavigation { get; set; }

    public virtual ICollection<Pallet> Pallet { get; set; } = new List<Pallet>();

    public virtual ICollection<Supply> SupplyIdUserReceiveNavigation { get; set; } = new List<Supply>();

    public virtual ICollection<Supply> SupplyIdUserSendNavigation { get; set; } = new List<Supply>();

    public virtual ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
}
