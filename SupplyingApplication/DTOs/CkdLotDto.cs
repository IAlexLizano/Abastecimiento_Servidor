namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para información de un CKD Lot
    /// Contiene los detalles del lote de vehículos recibido
    /// </summary>
    public class CkdLotResponseDto
    {
        public int IdLot { get; set; }
        public string CpaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? Version { get; set; }
        public int? Year { get; set; }
        public string? Sequence { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string? Status { get; set; }
        public IEnumerable<ContainerResponseDto>? Containers { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para filtrar CKD Lots por estado
    /// </summary>
    public class GetCkdLotsByStatusRequestDto
    {
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// DTO de solicitud para obtener CKD Lot con detalles completos
    /// </summary>
    public class GetCkdLotDetailsRequestDto
    {
        public int CkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para buscar CKD Lots por modelo
    /// </summary>
    public class GetCkdLotsByModelRequestDto
    {
        public string ModelPrefix { get; set; } = null!;
    }
}
