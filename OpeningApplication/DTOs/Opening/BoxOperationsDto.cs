namespace OpeningApplication.DTOs.Opening
{
    /// <summary>
    /// DTO de solicitud para obtener cajas por pallet
    /// </summary>
    public class GetBoxesByPalletRequestDto
    {
        public int PalletId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener cajas por usuario
    /// </summary>
    public class GetBoxesByUserRequestDto
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener una caja por ID
    /// </summary>
    public class GetBoxByIdRequestDto
    {
        public int BoxId { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para información completa de una caja con labeled_boxes
    /// </summary>
    public class BoxWithLabeledBoxesResponseDto
    {
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
        public string? Status { get; set; }
        public int? IdPallet { get; set; }
        public string? PalletCode { get; set; }
        public List<BoxProductDetailResponseDto>? ProductDetails { get; set; }
        public List<LabeledBoxDetailResponseDto>? LabeledBoxes { get; set; }
    }

    /// <summary>
    /// DTO de detalles de producto en caja
    /// </summary>
    public class BoxProductDetailResponseDto
    {
        public int IdBoxProductDetail { get; set; }
        public int IdComponent { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentDescription { get; set; }
        public string? ComponentName { get; set; }
        public int ExpectedQuantity { get; set; }
        public int RevisedQuantity { get; set; }
        public int? VerifiedQuantity { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    /// <summary>
    /// DTO de detalles de labeled_box
    /// </summary>
    public class LabeledBoxDetailResponseDto
    {
        public int IdLabeledBox { get; set; }
        public int IdStation { get; set; }
        public string? StationName { get; set; }
        public int Quantity { get; set; }
        public string? SupplyStatus { get; set; }
        public DateTime? SupplyDate { get; set; }
        public int? IdUser { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para registrar desempaque de cartón (unboxing)
    /// Solo requiere el ID del labeled_box a desempacar
    /// El usuario se obtiene del contexto global (InformationSession)
    /// </summary>
    public class RegisterUnboxingRequestDto
    {
        public int IdLabeledBox { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para desempaque registrado
    /// </summary>
    public class UnboxingRegisteredResponseDto
    {
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public List<LabeledBoxDetailResponseDto>? UpdatedLabeledBoxes { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para registrar apertura de caja (box opening)
    /// Solo requiere el ID de la caja. La cascada de verificación es automática.
    /// </summary>
    public class RegisterBoxOpeningRequestDto
    {
        public int IdBox { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para apertura de caja registrada
    /// </summary>
    public class BoxOpeningRegisteredResponseDto
    {
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int ComponentsProcessed { get; set; }
        public List<BoxProductDetailResponseDto>? UpdatedProductDetails { get; set; }
    }

    /// <summary>
    /// DTO de información básica de un lote (solo id y código)
    /// </summary>
    public class BasicLotInfoDto
    {
        public int IdLot { get; set; }
        public string LotCode { get; set; } = null!;
        public List<BasicContainerInfoDto> Containers { get; set; } 
    }

    /// <summary>
    /// DTO de información básica de un contenedor (solo id y código)
    /// </summary>
    public class BasicContainerInfoDto
    {
        public int IdContainer { get; set; }
        public string ContainerCode { get; set; } = null!;
        public List<BasicPalletInfoDto> Pallets { get; set; }
    }

    /// <summary>
    /// DTO de información básica de un pallet (solo id y código)
    /// </summary>
    public class BasicPalletInfoDto
    {
        public int IdPallet { get; set; }
        public string PalletCode { get; set; } = null!;
        public List<BasicBoxInfoDto> Boxes { get; set; }
    }

    /// <summary>
    /// DTO de información básica de una caja (solo id y código)
    /// </summary>
    public class BasicBoxInfoDto
    {
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
    }

    /// <summary>
    /// DTO de respuesta para información básica de una caja procesada hoy
    /// Información ligera sin detalles de labeled_boxes
    /// </summary>
    public class ProcessedBoxTodayResponseDto
    {
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
        public string? Status { get; set; }
        public int? IdPallet { get; set; }
        public string? PalletCode { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para información básica de todos los elementos en estado "en proceso"
    /// Contiene lotes, contenedores, pallets y cajas solo con id y código
    /// </summary>
    public class BasicInformationResponseDto
    {
        public List<BasicLotInfoDto> Lots { get; set; }
    }
}
