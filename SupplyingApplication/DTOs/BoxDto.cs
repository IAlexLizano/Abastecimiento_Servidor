namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para información de una Caja
    /// Contiene todos los componentes/detalles de productos de la caja
    /// </summary>
    public class BoxResponseDto
    {
        public int IdBox { get; set; }
        public int? IdPallet { get; set; }
        public string BoxCode { get; set; } = null!;
        public string? Status { get; set; }
        public IEnumerable<BoxProductDetailResponseDto>? ProductDetails { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener cajas de un pallet
    /// </summary>
    public class GetBoxesByPalletRequestDto
    {
        public int PalletId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener detalles completos de una caja
    /// </summary>
    public class GetBoxDetailsRequestDto
    {
        public int BoxId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para actualizar estado de una caja
    /// </summary>
    public class UpdateBoxStatusRequestDto
    {
        public int BoxId { get; set; }
        public string Status { get; set; } = null!;
    }
}
