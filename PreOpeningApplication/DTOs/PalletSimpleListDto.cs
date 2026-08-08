namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO para listar todos los pallets con información simplificada
    /// </summary>
    public class PalletSimpleListResponseDto
    {
        public List<PalletSimpleDto> Pallets { get; set; } = new();
    }

    /// <summary>
    /// DTO simple de pallet con id, code y content
    /// </summary>
    public class PalletSimpleDto
    {
        public int IdPallet { get; set; }
        public string PalletCode { get; set; } = null!;
        public string? Content { get; set; }
    }
}
