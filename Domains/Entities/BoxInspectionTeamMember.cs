namespace Domains.Entities;

public partial class BoxInspectionTeamMember
{
    public int IdTeam { get; set; }

    public int IdUser { get; set; }

    public string? AssignedColor { get; set; }

    public int IdTeamMember { get; set; }

    public virtual BoxInspectionTeam IdTeamNavigation { get; set; } = null!;

    public virtual RegisteredUser IdUserNavigation { get; set; } = null!;
}
