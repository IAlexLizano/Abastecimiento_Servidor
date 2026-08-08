namespace Domains.Entities;

public partial class Recipe
{
    public int IdRecipe { get; set; }

    public int IdComponent { get; set; }

    public int CantCar { get; set; }

    public int IdStation { get; set; }

    public string ProcessSheet { get; set; } = null!;

    public string? Ownership { get; set; }

    public int? Total { get; set; }

    public int IdModel { get; set; }

    public virtual ICollection<Cardboard> Cardboard { get; set; } = new List<Cardboard>();

    public virtual Component IdComponentNavigation { get; set; } = null!;

    public virtual Model IdModelNavigation { get; set; } = null!;

    public virtual WorkStation IdStationNavigation { get; set; } = null!;
}
