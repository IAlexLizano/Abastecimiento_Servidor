namespace GeneralApplication.DTOs.Dashboard
{
    public class ContainerStatusKpiDto
    {
        public int Received { get; set; }
        public int InProcess { get; set; }
        public int Processed { get; set; }
        public int Supplied { get; set; }
    }
}
