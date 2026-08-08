namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para formato de recepción completo
    /// Contiene información de distribución de componentes por estación
    /// </summary>
    public class ReceivingFormatResponseDto
    {
        public int CkdLotId { get; set; }
        public string CpaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? Version { get; set; }
        public int? Year { get; set; }
        public DateTime GeneratedDate { get; set; }
        public int TotalComponents { get; set; }
        public int TotalBoxes { get; set; }
        public int TotalPallets { get; set; }
        public List<StationDistributionResponseDto>? StationDistribution { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para distribución de componentes por estación
    /// </summary>
    public class StationDistributionResponseDto
    {
        public int StationId { get; set; }
        public string StationName { get; set; } = null!;
        public int ComponentCount { get; set; }
        public List<ComponentDetailResponseDto>? ComponentDetails { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para desglose de etiquetas por pallet
    /// </summary>
    public class LabelBreakdownResponseDto
    {
        public string CkdLotCode { get; set; } = null!;
        public string ContainerNum { get; set; } = null!;
        public string PalletCode { get; set; } = null!;
        public int BoxCount { get; set; }
        public List<BoxLabelResponseDto>? Boxes { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para información de etiqueta de caja
    /// </summary>
    public class BoxLabelResponseDto
    {
        public string BoxCode { get; set; } = null!;
        public string BoxStatus { get; set; } = null!;
    }

    /// <summary>
    /// DTO de solicitud para generar formato de recepción
    /// </summary>
    public class GenerateReceivingFormatRequestDto
    {
        public int CkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para generar desglose de etiquetas
    /// </summary>
    public class GenerateLabelBreakdownRequestDto
    {
        public int CkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener componentes agrupados por estación
    /// </summary>
    public class GetComponentsByStationRequestDto
    {
        public int CkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener componentes de una estación específica
    /// </summary>
    public class GetComponentsByStationDetailedRequestDto
    {
        public int CkdLotId { get; set; }
        public int StationId { get; set; }
    }
}
