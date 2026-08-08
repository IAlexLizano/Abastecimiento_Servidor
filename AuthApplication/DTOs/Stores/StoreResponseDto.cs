namespace AuthApplication.DTOs.Stores
{
    /// <summary>
    /// DTO para la respuesta de una tienda
    /// </summary>
    public class StoreResponseDto
    {
        public int IdStore { get; set; }

        public string StoreName { get; set; } = null!;

        public string StoreCode { get; set; } = null!;
    }
}
