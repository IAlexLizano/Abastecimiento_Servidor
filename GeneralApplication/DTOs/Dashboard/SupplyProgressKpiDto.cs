namespace GeneralApplication.DTOs.Dashboard
{
    public class SupplyProgressKpiDto
    {
        public string StationName { get; set; } = string.Empty;
        public int Processed { get; set; }
        public int Pending { get; set; }
        public int Supplied { get; set; }
    }
}
