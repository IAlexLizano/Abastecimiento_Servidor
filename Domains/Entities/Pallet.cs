namespace Domains.Entities;

public partial class Pallet
{
    public int IdPallet { get; set; }

    public int IdContainer { get; set; }

    public string PalletNumber { get; set; } = null!;

    public string? Status { get; set; }

    public string? Content { get; set; }

    public string? Description { get; set; }

    public bool? NeedClaim { get; set; }

    public int? IdUser { get; set; }

    public virtual ICollection<Box> Box { get; set; } = new List<Box>();

    public virtual Container IdContainerNavigation { get; set; } = null!;

    public virtual RegisteredUser? IdUserNavigation { get; set; }
}
