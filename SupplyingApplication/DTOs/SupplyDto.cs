namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para información básica de un Supply
    /// </summary>
    public class SupplyDto
    {
        public int IdSupply { get; set; }
        public int? IdCardboard { get; set; }
        public int? IdUserSend { get; set; }
        public int? IdUserReceive { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? SupplyStatus { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para abastecer una estación
    /// </summary>
    public class SupplyStationRequestDto
    {
        public int IdSupply { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para recibir un cartón
    /// </summary>
    public class ReceiveBoxRequestDto
    {
        public int IdSupply { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para operación de abastecimiento
    /// </summary>
    public class SupplyOperationResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public SupplyDto? Data { get; set; }
    }
}
