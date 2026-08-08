namespace Domains.Entities;

public partial class Component
{
    public int IdComponent { get; set; }

    public string PartCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? TechParameters { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<BoxProductDetail> BoxProductDetail { get; set; } = new List<BoxProductDetail>();

    public virtual ICollection<Recipe> Recipe { get; set; } = new List<Recipe>();
}
