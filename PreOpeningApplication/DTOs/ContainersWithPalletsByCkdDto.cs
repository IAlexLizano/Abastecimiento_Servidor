namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO para obtener todos los contenedores con sus pallets por CKD Lot
    /// </summary>
    public class ContainersWithPalletsByCkdResponseDto
    {
        public int IdLot { get; set; }
        public string LotCode { get; set; } = null!;
        public string Product { get; set; } = null!;
        public DateTime ArrivalDate { get; set; }
        public string Status { get; set; } = null!;
        public List<ContainerWithPalletsDetailDto> Containers { get; set; } = new();
    }

    /// <summary>
    /// DTO de detalle de contenedor con sus pallets
    /// </summary>
    public class ContainerWithPalletsDetailDto
    {
        public int IdContainer { get; set; }
        public string ContainerCode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int IdStore { get; set; }
        public string? StoreName { get; set; }
        public List<PalletDetailDto> Pallets { get; set; } = new();
    }
}
