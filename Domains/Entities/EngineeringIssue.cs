namespace Domains.Entities;

public partial class EngineeringIssue
{
    public int IdIssue { get; set; }

    public int? IdDetail { get; set; }

    public int? IdUserReports { get; set; }

    public string Reason { get; set; } = null!;

    public string? ResolutionStatus { get; set; }

    public DateTime? ReportDate { get; set; }

    public int? IdUserSolves { get; set; }

    public DateTime? SolveDate { get; set; }

    public virtual BoxProductDetail? IdDetailNavigation { get; set; }

    public virtual RegisteredUser? IdUserReportsNavigation { get; set; }

    public virtual RegisteredUser? IdUserSolvesNavigation { get; set; } = null!;
}
