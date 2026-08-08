namespace Domains.Entities;

public partial class Service
{
    public int IdService { get; set; }

    public string Name { get; set; } = null!;

    public string ServiceKey { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<AccessControl> AccessControl { get; set; } = new List<AccessControl>();
}
