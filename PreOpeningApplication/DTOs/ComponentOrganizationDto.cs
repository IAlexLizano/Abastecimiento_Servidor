namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para componentes agrupados por estación
    /// Contiene la distribución de componentes de un CKD por estaciones de trabajo
    /// </summary>
    public class ComponentStationGroupDto
    {
        public int StationId { get; set; }
        public string StationName { get; set; } = null!;
        public int ComponentCount { get; set; }
        public List<ComponentDetailDto>? Components { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para detalles de componente en organizacion
    /// </summary>
    public class ComponentDetailDto
    {
        public int DetailId { get; set; }
        public int? ComponentId { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentDescription { get; set; }
        public int OrderedQuantity { get; set; }
        public int? StationId { get; set; }
        public string? StationName { get; set; }
        public bool? RequiresEngineering { get; set; }
    }
}
