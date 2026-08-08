namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para resultado de comparación de componentes entre modelos
    /// Contiene información sobre nuevos, faltantes y cambios de cantidad
    /// </summary>
    public class ComponentComparisonResponseDto
    {
        public string CurrentModelPrefix { get; set; } = null!;
        public string PreviousModelPrefix { get; set; } = null!;
        public bool ComponentsMatch { get; set; }
        public int NewComponentCount { get; set; }
        public int MissingComponentCount { get; set; }
        public int ChangedComponentCount { get; set; }
        public bool RequiresEngineering { get; set; }
        public List<ComponentDetailResponseDto>? NewComponents { get; set; }
        public List<ComponentDetailResponseDto>? MissingComponents { get; set; }
        public List<ComponentChangeResponseDto>? ComponentChanges { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para detalles de componente
    /// </summary>
    public class ComponentDetailResponseDto
    {
        public int DetailId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentCode { get; set; } = null!;
        public string ComponentDescription { get; set; } = null!;
        public int OrderedQuantity { get; set; }
        public bool RequiresEngineering { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para cambios detectados en componentes
    /// </summary>
    public class ComponentChangeResponseDto
    {
        public int ComponentId { get; set; }
        public string ComponentCode { get; set; } = null!;
        public string ChangeType { get; set; } = null!;
        public int? PreviousQuantity { get; set; }
        public int? CurrentQuantity { get; set; }
        public int? QuantityDifference { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para comparar componentes entre modelos
    /// </summary>
    public class CompareComponentsRequestDto
    {
        public int CurrentCkdLotId { get; set; }
        public int PreviousCkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener diferencias de componentes entre aperturas
    /// </summary>
    public class GetComponentDifferencesRequestDto
    {
        public int CurrentCkdLotId { get; set; }
        public int PreviousCkdLotId { get; set; }
    }

    /// <summary>
    /// Alias para ComponentComparisonResponseDto
    /// </summary>
    public class ComponentComparisonResultDto : ComponentComparisonResponseDto { }

    /// <summary>
    /// DTO para cambios detectados en componentes (alias)
    /// </summary>
    public class ComponentChangeDto : ComponentChangeResponseDto { }
}
