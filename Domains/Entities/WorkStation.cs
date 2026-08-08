namespace Domains.Entities;

public partial class WorkStation
{
    public int IdStation { get; set; }

    public string Name { get; set; } = null!;

    public string CodeStation { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<Recipe> Recipe { get; set; } = new List<Recipe>();

    public virtual ICollection<RegisteredUser> RegisteredUser { get; set; } = new List<RegisteredUser>();
}
