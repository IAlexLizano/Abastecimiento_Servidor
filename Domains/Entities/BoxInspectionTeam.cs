namespace Domains.Entities;

public partial class BoxInspectionTeam
{
    public int IdTeam { get; set; }

    public int? IdBox { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BoxInspectionTeamMember> BoxInspectionTeamMember { get; set; } = new List<BoxInspectionTeamMember>();

    public virtual Box? IdBoxNavigation { get; set; }
}
