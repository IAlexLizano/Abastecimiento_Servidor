namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO para obtener todos los pallets de un contenedor
    /// </summary>
    public class PalletsByContainerResponseDto
    {
        public int IdContainer { get; set; }
        public string ContainerCode { get; set; } = null!;
        public int IdLot { get; set; }
        public string Status { get; set; } = null!;
        public int IdStore { get; set; }
        public string? StoreName { get; set; }
        public List<PalletDetailDto> Pallets { get; set; } = new();
    }

    /// <summary>
    /// DTO de detalle de pallet
    /// </summary>
    public class PalletDetailDto
    {
        public int IdPallet { get; set; }
        public string PalletCode { get; set; } = null!;
        public string? Content { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public bool Claim { get; set; }
        public int IdUserInCharge { get; set; }
        public string? UserInChargeName { get; set; }
    }
}
