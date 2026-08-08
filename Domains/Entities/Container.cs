namespace Domains.Entities;

public partial class Container
{
    public int IdContainer { get; set; }

    public int IdLot { get; set; }

    public string ContainerNumber { get; set; } = null!;

    public string? Status { get; set; }

    public int? IdStore { get; set; }

    public DateTime? DisembarkationDate { get; set; }

    public string? StampNumber { get; set; }

    public virtual Lot IdLotNavigation { get; set; } = null!;

    public virtual Store? IdStoreNavigation { get; set; }

    public virtual ICollection<Pallet> Pallet { get; set; } = new List<Pallet>();
}
