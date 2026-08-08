namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO de solicitud para poner contenedores en bodega
    /// </summary>
    public class StoreContainerRequestDto
    {
        public int IdContainer { get; set; }
        public int IdStore { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para contenedor puesto en bodega
    /// </summary>
    public class StoreContainerResponseDto
    {
        public int IdContainer { get; set; }
        public string ContainerCode { get; set; } = null!;
        public int IdStore { get; set; }
        public string StoreName { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// DTO para formato de descarga de contenedores
    /// Retorna contenedores con pallets en bodega interna (ESTACION)
    /// </summary>
    public class ContainerUnloadingFormatDto
    {
        public int CkdLotId { get; set; }
        public string LotCode { get; set; } = null!;
        public string Product { get; set; } = null!;
        public List<ContainerUnloadingDetailDto> Containers { get; set; } = new();
    }

    /// <summary>
    /// DTO de detalle de contenedor para descarga
    /// </summary>
    public class ContainerUnloadingDetailDto
    {
        public int IdContainer { get; set; }
        public string ContainerCode { get; set; } = null!;
        public string? StoreName { get; set; }
        public string UnloadingDate { get; set; } = null!;
        public int PalletCount { get; set; }
        public List<PalletUnloadingDetailDto> Pallets { get; set; } = new();
    }

    /// <summary>
    /// DTO de detalle de pallet para descarga de contenedor
    /// </summary>
    public class PalletUnloadingDetailDto
    {
        public int IdPallet { get; set; }
        public string PalletCode { get; set; } = null!;
        public string? Content { get; set; }
        public string? Description { get; set; }
        public bool? Claim { get; set; }
        public int BoxCount { get; set; }
        public string? UserName { get; set; }
    }
}
