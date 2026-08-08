namespace ComponentsApplication.DTOs
{
    public class ComponentResponseDto
    {
        public int IdComponent { get; set; }

        public string PartCode { get; set; }

        public string Description { get; set; }

        public string TechParameters { get; set; }

        public string? ImagePath { get; set; }
    }
}