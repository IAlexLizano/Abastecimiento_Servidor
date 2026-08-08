namespace ComponentsApplication.DTOs
{
    public class CreateComponentRequestDto
    {
        public string PartCode { get; set; }

        public string Description { get; set; }

        public string? TechParameters { get; set; }

        public string? ImagePath { get; set; }
    }
}
