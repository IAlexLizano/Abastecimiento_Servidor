using System.Collections.Generic;

namespace GeneralApplication.DTOs.Dashboard
{
    public class DashboardKpisResponseDto
    {
        public List<KpiStatusDto> ContainerStatus { get; set; } = new List<KpiStatusDto>();
        public List<KpiStatusDto> BoxOpening { get; set; } = new List<KpiStatusDto>();
        public List<SupplyProgressKpiDto> SupplyProgress { get; set; } = new List<SupplyProgressKpiDto>();
        public List<InspectionLoadKpiDto> InspectionLoad { get; set; } = new List<InspectionLoadKpiDto>();
        public List<KpiStatusDto> IncidentTracking { get; set; } = new List<KpiStatusDto>();
    }
}
