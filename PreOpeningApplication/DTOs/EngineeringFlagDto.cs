namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO de solicitud para actualizar bandera de ingeniería de un componente
    /// </summary>
    public class UpdateEngineeringFlagRequestDto
    {
        public int DetailId { get; set; }
        public bool RequiresEngineering { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener componentes que requieren ingeniería
    /// </summary>
    public class GetComponentsRequiringEngineeringRequestDto
    {
        public int CkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para componentes que requieren ingeniería
    /// </summary>
    public class EngineeringRequiredResponseDto
    {
        public int DetailId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentCode { get; set; } = null!;
        public string ComponentDescription { get; set; } = null!;
        public int? StationId { get; set; }
        public string? StationName { get; set; }
        public int OrderedQuantity { get; set; }
        public string? ReasonForEngineering { get; set; }
    }
}
