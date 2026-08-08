namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO para listar todos los contenedores con información simplificada
    /// </summary>
    public class ContainerSimpleListResponseDto
    {
        public List<ContainerSimpleDto> Containers { get; set; } = new();
    }

    /// <summary>
    /// DTO simple de contenedor con id y code
    /// </summary>
    public class ContainerSimpleDto
    {
        public int IdContainer { get; set; }
        public string ContainerCode { get; set; } = null!;
        public int IdStore { get; set; }
    }
}
