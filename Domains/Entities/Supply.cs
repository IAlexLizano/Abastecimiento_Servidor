namespace Domains.Entities;

public partial class Supply
{
    public int IdSupply { get; set; }

    public int? IdUserSend { get; set; }

    public DateTime? SendDate { get; set; }

    public int? IdUserReceive { get; set; }

    public DateTime? ReceiveDate { get; set; }

    public int? IdCardboard { get; set; }

    public string SupplyStatus { get; set; } = null!;

    public virtual Cardboard? IdCardboardNavigation { get; set; }

    public virtual RegisteredUser? IdUserReceiveNavigation { get; set; }

    public virtual RegisteredUser? IdUserSendNavigation { get; set; }
}
