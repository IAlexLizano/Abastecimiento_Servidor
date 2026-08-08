namespace SupplyingApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para estación de trabajo
    /// Reemplaza la entidad WorkStation del Domain
    /// </summary>
    public class WorkStationDto
    {
        public int IdStation { get; set; }
        public string StationName { get; set; } = null!;
        public string? StationType { get; set; }
        public bool? IsActive { get; set; }
    }
}
