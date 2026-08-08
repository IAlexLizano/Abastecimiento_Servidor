namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO para cargar un lote completo con todos sus detalles
    /// </summary>
    public class LoadCkdLotRequestDto
    {
        public string LotCode { get; set; } = null!;
        public string Product { get; set; } = null!;
        public DateTime? ArrivalDate { get; set; }
        public List<LoadContainerRequestDto> Containers { get; set; } = new();
    }

    /// <summary>
    /// DTO para cargar un contenedor con sus pallets
    /// </summary>
    public class LoadContainerRequestDto
    {
        public string ContainerCode { get; set; } = null!;
        public string? StampNumber { get; set; }
        public List<LoadPalletRequestDto> Pallets { get; set; } = new();
    }

    /// <summary>
    /// DTO para cargar un pallet con sus cajas
    /// </summary>
    public class LoadPalletRequestDto
    {
        public string PalletCode { get; set; } = null!;
        public List<LoadBoxRequestDto> Boxes { get; set; } = new();
    }

    /// <summary>
    /// DTO para cargar una caja con sus detalles de componentes
    /// </summary>
    public class LoadBoxRequestDto
    {
        public string BoxCode { get; set; } = null!;
        public List<LoadBoxProductDetailRequestDto> ProductDetails { get; set; } = new();
    }

    /// <summary>
    /// DTO para cargar detalles de producto en una caja
    /// </summary>
    public class LoadBoxProductDetailRequestDto
    {
        public string ComponentCode { get; set; } = null!;
        public int TotalQuantity { get; set; }
        public int PackingQuantity { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para lote completo con todos sus detalles
    /// </summary>
    public class CkdLotFullDetailsDto
    {
        public int IdLot { get; set; }
        public string LotCode { get; set; } = null!;
        public string Product { get; set; } = null!;
        public DateTime? ArrivalDate { get; set; }
        public string? Status { get; set; }
        public List<ContainerFullDetailsDto> Containers { get; set; } = new();
    }

    /// <summary>
    /// DTO de contenedor completo
    /// </summary>
    public class ContainerCompleteDto
    {
        public int IdContainer { get; set; }
        public string ContainerCode { get; set; } = null!;
        public string? Status { get; set; }
        public int? IdStore { get; set; }
        public string? StoreName { get; set; }
        public List<PalletCompleteDto> Pallets { get; set; } = new();
    }

    /// <summary>
    /// DTO de contenedor con todos sus detalles
    /// </summary>
    public class ContainerFullDetailsDto
    {
        public int IdContainer { get; set; }
        public string ContainerCode { get; set; } = null!;
        public string? Status { get; set; }
        public int? IdStore { get; set; }
        public string? StoreName { get; set; }
        public List<PalletFullDetailsDto> Pallets { get; set; } = new();
    }

    /// <summary>
    /// DTO de pallet completo
    /// </summary>
    public class PalletCompleteDto
    {
        public int IdPallet { get; set; }
        public string PalletCode { get; set; } = null!;
        public string? Status { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public bool? Claim { get; set; }
        public int IdUserInCharge { get; set; }
        public string? UserInChargeName { get; set; }
        public List<BoxCompleteDto> Boxes { get; set; } = new();
    }

    /// <summary>
    /// DTO de pallet con todos sus detalles
    /// </summary>
    public class PalletFullDetailsDto
    {
        public int IdPallet { get; set; }
        public string PalletCode { get; set; } = null!;
        public string? Status { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public bool? Claim { get; set; }
        public int IdUserInCharge { get; set; }
        public string? UserInChargeName { get; set; }
        public List<BoxFullDetailsDto> Boxes { get; set; } = new();
    }

    /// <summary>
    /// DTO de caja completa
    /// </summary>
    public class BoxCompleteDto
    {
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
        public string? Status { get; set; }
        public List<BoxProductDetailCompleteDto> ProductDetails { get; set; } = new();
    }

    /// <summary>
    /// DTO de caja con todos sus detalles
    /// </summary>
    public class BoxFullDetailsDto
    {
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
        public string? Status { get; set; }
        public List<BoxProductDetailFullDto> ProductDetails { get; set; } = new();
    }

    /// <summary>
    /// DTO de detalles de producto en caja
    /// </summary>
    public class BoxProductDetailCompleteDto
    {
        public int IdBoxProductDetail { get; set; }
        public int IdComponent { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentDescription { get; set; }
        public int ExpectedQuantity { get; set; }
        public int RevisedQuantity { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    /// <summary>
    /// DTO de detalles de producto en caja con información completa
    /// </summary>
    public class BoxProductDetailFullDto
    {
        public int IdBoxProductDetail { get; set; }
        public int IdComponent { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentDescription { get; set; }
        public int ExpectedQuantity { get; set; }
        public int RevisedQuantity { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
