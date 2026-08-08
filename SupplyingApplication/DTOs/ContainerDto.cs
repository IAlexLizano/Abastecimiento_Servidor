namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para información de un Contenedor
    /// Contiene los pallets que conforman el contenedor de un CKD
    /// </summary>
    public class ContainerResponseDto
    {
        public int IdContainer { get; set; }
        public int? IdLot { get; set; }
        public string ContainerNum { get; set; } = null!;
        public string? ContainerCode { get; set; }
        public string? Status { get; set; }
        public IEnumerable<PalletResponseDto>? Pallets { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener contenedores de un CKD Lot
    /// </summary>
    public class GetContainersByCkdRequestDto
    {
        public int CkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para actualizar estado del contenedor
    /// </summary>
    public class UpdateContainerStatusRequestDto
    {
        public int ContainerId { get; set; }
    }
}
