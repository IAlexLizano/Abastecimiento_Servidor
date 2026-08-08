namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para movimientos en el Kardex de suministro
    /// Contiene el historial de movimientos de un componente en una estación
    /// </summary>
    public class SupplyKardexResponseDto
    {
        public int IdKardex { get; set; }
        public int? IdComponent { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentDescription { get; set; }
        public int? IdStation { get; set; }
        public string? StationName { get; set; }
        public int? IdUser { get; set; }
        public string? Username { get; set; }
        public string MovementType { get; set; } = null!;
        public int Quantity { get; set; }
        public int CurrentBalance { get; set; }
        public DateTime? MovementDate { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para transferir componentes a una estación de trabajo
    /// </summary>
    public class TransferComponentsRequestDto
    {
        public int ComponentId { get; set; }
        public int StationId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener kardex de un componente
    /// </summary>
    public class GetSupplyKardexRequestDto
    {
        public int ComponentId { get; set; }
        public int? StationId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener balance actual de un componente en una estación
    /// </summary>
    public class GetComponentBalanceRequestDto
    {
        public int ComponentId { get; set; }
        public int StationId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para registrar movimiento en kardex
    /// </summary>
    public class RecordKardexMovementRequestDto
    {
        public int ComponentId { get; set; }
        public int StationId { get; set; }
        public int UserId { get; set; }
        public string MovementType { get; set; } = null!; // ENTRADA, SALIDA, AJUSTE
        public int Quantity { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para balance actual de inventario
    /// </summary>
    public class ComponentBalanceResponseDto
    {
        public int ComponentId { get; set; }
        public string? ComponentCode { get; set; }
        public int StationId { get; set; }
        public string? StationName { get; set; }
        public int CurrentBalance { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para operación de transferencia exitosa
    /// </summary>
    public class TransferResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public int ComponentId { get; set; }
        public int StationId { get; set; }
        public int TransferredQuantity { get; set; }
        public int NewBalance { get; set; }
    }
}
