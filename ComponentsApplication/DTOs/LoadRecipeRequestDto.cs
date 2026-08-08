namespace ComponentsApplication.DTOs
{
    public class LoadRecipeRequestDto
    {
        public ComponentSimpleDto Component { get; set; } = null!;

        public int CantCar { get; set; }

        public string StationCode { get; set; } = null!;

        public string ProcessSheet { get; set; } = null!;

        public string? Ownership { get; set; }

        public int? Total { get; set; }
    }

    public class ComponentSimpleDto
    {
        public string Code { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? TechParameters { get; set; }

        public string? ImagePath { get; set; }
    }
}
