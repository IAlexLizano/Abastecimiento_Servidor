namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para información de un Pallet
    /// Contiene todas las cajas que conforman el pallet
    /// </summary>
    public class PalletResponseDto
    {
        public int IdPallet { get; set; }
        public int? IdContainer { get; set; }
        public string PalletCode { get; set; } = null!;
        public string? Status { get; set; }
        public IEnumerable<BoxResponseDto>? Boxes { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener pallets de un contenedor
    /// </summary>
    public class GetPalletsByContainerRequestDto
    {
        public int ContainerId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener detalles completos de un pallet
    /// </summary>
    public class GetPalletDetailsRequestDto
    {
        public int PalletId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para actualizar estado de un pallet
    /// </summary>
    public class UpdatePalletStatusRequestDto
    {
        public int PalletId { get; set; }
    }
}
