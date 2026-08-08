namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para formato de recepción completo
    /// Contiene información de distribución de componentes por estación
    /// </summary>
    public class ReceivingFormatDto
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
        public List<StationDistributionDto>? StationDistribution { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para distribución de componentes por estación
    /// </summary>
    public class StationDistributionDto
    {
        public int StationId { get; set; }
        public string StationName { get; set; } = null!;
        public int ComponentCount { get; set; }
        public List<ComponentDetailDto>? ComponentDetails { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para desglose de etiquetas por pallet
    /// </summary>
    public class LabelBreakdownDto
    {
        public string CkdLotCode { get; set; } = null!;
        public string ContainerNum { get; set; } = null!;
        public string PalletCode { get; set; } = null!;
        public int BoxCount { get; set; }
        public List<BoxLabelDto>? Boxes { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para información de etiqueta de caja
    /// </summary>
    public class BoxLabelDto
    {
        public string BoxCode { get; set; } = null!;
        public string BoxStatus { get; set; } = null!;
        public int ComponentCount { get; set; }
        public List<ComponentLabelDto>? Components { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para información de etiqueta de componente
    /// </summary>
    public class ComponentLabelDto
    {
        public int? ComponentId { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentDescription { get; set; }
        public int OrderedQuantity { get; set; }
        public int? StationId { get; set; }
        public string? StationName { get; set; }
    }
}
