namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO para listar todos los lotes CKD con información simplificada
    /// </summary>
    public class CkdLotSimpleListResponseDto
    {
        public List<CkdLotSimpleDto> Lots { get; set; } = new();
    }

    /// <summary>
    /// DTO simple de lote CKD con solo id, code y product
    /// </summary>
    public class CkdLotSimpleDto
    {
        public int IdLot { get; set; }
        public string LotCode { get; set; } = null!;
        public string Product { get; set; } = null!;
    }
}
