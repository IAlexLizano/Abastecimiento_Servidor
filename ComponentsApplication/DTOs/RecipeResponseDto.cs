namespace ComponentsApplication.DTOs
{
    public class RecipeResponseDto
    {
        public int IdRecipe { get; set; }

        public ComponentSimpleDto Component { get; set; } = null!;

        public int CantCar { get; set; }

        public string StationCode { get; set; } = null!;

        public string ProcessSheet { get; set; } = null!;

        public string? Ownership { get; set; }

        public int? Total { get; set; }
    }
}
