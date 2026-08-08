namespace Domains.Entities;

public partial class Box
{
    public int IdBox { get; set; }

    public int? IdPallet { get; set; }

    public string BoxNumber { get; set; } = null!;

    public string? Status { get; set; }

    public virtual ICollection<BoxInspectionTeam> BoxInspectionTeam { get; set; } = new List<BoxInspectionTeam>();

    public virtual ICollection<BoxProductDetail> BoxProductDetail { get; set; } = new List<BoxProductDetail>();

    public virtual Pallet? IdPalletNavigation { get; set; }
}
