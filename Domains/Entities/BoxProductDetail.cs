namespace Domains.Entities;

public partial class BoxProductDetail
{
    public int IdBoxProductDetail { get; set; }

    public int IdComponent { get; set; }

    public int IdBox { get; set; }

    public int TotalQuantity { get; set; }

    public int RevisedQuantity { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int PackingQuantity { get; set; }

    public virtual ICollection<Cardboard> Cardboard { get; set; } = new List<Cardboard>();

    public virtual ICollection<EngineeringIssue> EngineeringIssue { get; set; } = new List<EngineeringIssue>();

    public virtual Box IdBoxNavigation { get; set; } = null!;

    public virtual Component IdComponentNavigation { get; set; } = null!;
}
