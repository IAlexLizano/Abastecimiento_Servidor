namespace Domains.Entities;

public partial class Model
{
    public int IdModel { get; set; }

    public string ModelCode { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public int? Version { get; set; }

    public DateOnly? IssueDate { get; set; }

    public virtual ICollection<Recipe> Recipe { get; set; } = new List<Recipe>();
}
