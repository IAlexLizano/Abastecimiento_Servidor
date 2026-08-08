namespace Domains.Entities;

public partial class Lot
{
    public int IdLot { get; set; }

    public string LotCode { get; set; } = null!;

    public string Product { get; set; } = null!;

    public DateTime? ArrivalDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Container> Container { get; set; } = new List<Container>();
}
