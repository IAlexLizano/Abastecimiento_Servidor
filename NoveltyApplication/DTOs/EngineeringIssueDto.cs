namespace NoveltyApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para un issue de ingeniería (novedad)
    /// Reemplaza la entidad EngineeringIssue del Domain
    /// </summary>
    public class EngineeringIssueDto
    {
        public int IssueId { get; set; }
        public int? DetailId { get; set; }
        public int? ComponentId { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentDescription { get; set; }
        public int? BoxId { get; set; }
        public string? BoxCode { get; set; }
        public int? TotalQuantity { get; set; }
        public int? RevisedQuantity { get; set; }
        public int? ReportedByUserId { get; set; }
        public string? ReportedByUsername { get; set; }
        public string? ReportedByFullName { get; set; }
        public string Reason { get; set; } = null!;
        public string? ResolutionStatus { get; set; }
        public DateTime? ReportDate { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para registrar una novedad
    /// </summary>
    public class RegisterNoveltyRequestDto
    {
        public int BoxId { get; set; }
        public int ComponentId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public string Reason { get; set; } = null!;
    }
}
