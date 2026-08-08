namespace NoveltyApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para estadísticas generales de novedades
    /// Contiene resumen de novedades por estado y tasa de resolución
    /// </summary>
    public class NoveltyStatisticsResponseDto
    {
        public int TotalNovelties { get; set; }
        public int ReportedNovelties { get; set; }
        public int UnderReviewNovelties { get; set; }
        public int ResolvedNovelties { get; set; }
        public int PendingNovelties { get; set; }
        public double ResolutionRate { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para estadísticas de novedades por estación
    /// </summary>
    public class NoveltyStatisticsByStationResponseDto
    {
        public string StationName { get; set; } = null!;
        public int TotalNovelties { get; set; }
        public int ReportedNovelties { get; set; }
        public int UnderReviewNovelties { get; set; }
        public int ResolvedNovelties { get; set; }
        public int PendingNovelties { get; set; }
        public double ResolutionRate { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para estadísticas de novedades por componente
    /// </summary>
    public class NoveltyStatisticsByComponentResponseDto
    {
        public string ComponentCode { get; set; } = null!;
        public string ComponentDescription { get; set; } = null!;
        public int TotalNovelties { get; set; }
        public int ReportedNovelties { get; set; }
        public int UnderReviewNovelties { get; set; }
        public int ResolvedNovelties { get; set; }
        public int PendingNovelties { get; set; }
        public double ResolutionRate { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para obtener estadísticas generales
    /// </summary>
    public class GetGeneralStatisticsRequestDto
    {
    }

    /// <summary>
    /// DTO de solicitud para obtener estadísticas por estación
    /// </summary>
    public class GetStatisticsByStationRequestDto
    {
    }

    /// <summary>
    /// DTO de solicitud para obtener estadísticas por componente
    /// </summary>
    public class GetStatisticsByComponentRequestDto
    {
    }
}
