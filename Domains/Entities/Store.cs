namespace Domains.Entities;

public partial class Store
{
    public int IdStore { get; set; }

    public string StoreName { get; set; } = null!;

    public string StoreCode { get; set; } = null!;

    public virtual ICollection<Container> Container { get; set; } = new List<Container>();
}
