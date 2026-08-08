namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para detalles de productos en una caja
    /// Contiene información sobre componentes específicos asignados a una caja
    /// </summary>
    public class BoxProductDetailResponseDto
    {
        public int IdDetail { get; set; }
        public int? IdBox { get; set; }
        public int? IdComponent { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentDescription { get; set; }
        public int? IdStation { get; set; }
        public string? StationName { get; set; }
        public int OrderedQty { get; set; }
        public int? VerifiedQty { get; set; }
        public int? RemainingQty { get; set; }
        public int? SavedQty { get; set; }
        public string? SupplyStatus { get; set; }
        public bool? RequiresEngineering { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener detalles de productos de una caja
    /// </summary>
    public class GetBoxProductDetailsRequestDto
    {
        public int BoxId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener un detalle específico de producto
    /// </summary>
    public class GetBoxProductDetailRequestDto
    {
        public int DetailId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para actualizar el estado de abastecimiento de un detalle de producto
    /// </summary>
    public class UpdateBoxProductDetailStatusRequestDto
    {
        public int DetailId { get; set; }
        public string SupplyStatus { get; set; } = null!;
        public int? RemainingQty { get; set; }
        public int? SavedQty { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para registrar cantidad verificada de un componente
    /// </summary>
    public class UpdateVerifiedQtyRequestDto
    {
        public int DetailId { get; set; }
        public int VerifiedQty { get; set; }
    }
}
