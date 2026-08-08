namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para un lote CKD
    /// Reemplaza la entidad CkdLot del Domain
    /// </summary>
    public class CkdLotDto
    {
        public int IdLot { get; set; }
        public string CpaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? Version { get; set; }
        public int? Year { get; set; }
        public string? Sequence { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string? Status { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para detalle de producto en caja
    /// Reemplaza la entidad BoxProductDetail del Domain
    /// </summary>
    public class BoxProductDetailDto
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
}
