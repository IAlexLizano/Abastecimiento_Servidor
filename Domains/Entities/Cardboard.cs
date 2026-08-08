namespace Domains.Entities;

public partial class Cardboard
{
    public int IdCardboard { get; set; }

    public int IdBox { get; set; }

    public string? Status { get; set; }

    public int? IdUser { get; set; }

    public DateTime? OpeningDate { get; set; }

    public int? IdRecipe { get; set; }

    public virtual BoxProductDetail IdBoxNavigation { get; set; } = null!;

    public virtual Recipe? IdRecipeNavigation { get; set; }

    public virtual RegisteredUser? IdUserNavigation { get; set; }

    public virtual ICollection<Supply> Supply { get; set; } = new List<Supply>();
}
