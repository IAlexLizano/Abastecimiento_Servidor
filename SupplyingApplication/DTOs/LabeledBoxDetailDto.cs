namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para información detallada de un Cartón (Cardboard)
    /// Incluye información del componente y código de pallet
    /// </summary>
    public class LabeledBoxDetailDto
    {
        public int IdLabeledBox { get; set; }
        public int IdBox { get; set; }
        public int IdStation { get; set; }
        public int Quantity { get; set; }
        public string? SupplyStatus { get; set; }
        public DateTime? SupplyDate { get; set; }
        public int? IdUser { get; set; }
        public string? UserName { get; set; }

        /// <summary>
        /// Nombre de la estación
        /// </summary>
        public string? StationName { get; set; }

        /// <summary>
        /// Detalles de componentes del cartón
        /// </summary>
        public IEnumerable<BoxProductDetailResponseDto>? ProductDetails { get; set; }

        /// <summary>
        /// Código de pallet asociado
        /// </summary>
        public string? PalletCode { get; set; }

        /// <summary>
        /// Código del cartón
        /// </summary>
        public string? BoxCode { get; set; }

        /// <summary>
        /// Supplies asociados a este Cardboard
        /// </summary>
        public IEnumerable<SupplyDetailDto>? Supplies { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para detalles de un Supply
    /// </summary>
    public class SupplyDetailDto
    {
        public int IdSupply { get; set; }
        public int? IdCardboard { get; set; }
        public int? IdUserSend { get; set; }
        public int? IdUserReceive { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? SupplyStatus { get; set; }
        public string? SendUserName { get; set; }
        public string? ReceiveUserName { get; set; }
        public string? StationName { get; set; }
    }

    /// <summary>
    /// DTO para solicitud de obtener cartones pendientes
    /// </summary>
    public class GetPendingBoxesRequestDto
    {
    }

    /// <summary>
    /// DTO para solicitud de obtener cajas en proceso
    /// </summary>
    public class GetSuppliesInProgressRequestDto
    {
    }

    /// <summary>
    /// DTO para solicitud de obtener todos los cartones
    /// </summary>
    public class GetAllBoxesRequestDto
    {
    }

    /// <summary>
    /// DTO de respuesta para listado de cartones
    /// </summary>
    public class BoxListResponseDto
    {
        public IEnumerable<LabeledBoxDetailDto> Boxes { get; set; } = new List<LabeledBoxDetailDto>();
        public int TotalCount { get; set; }
    }
}
