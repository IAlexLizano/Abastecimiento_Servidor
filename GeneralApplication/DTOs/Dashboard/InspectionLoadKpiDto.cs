namespace GeneralApplication.DTOs.Dashboard
{
    public class InspectionLoadKpiDto
    {
        public string OperatorName { get; set; } = string.Empty;
        public int ManagedBoxes { get; set; }
        public double Percentage { get; set; }
    }
}
