namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO para solicitud de pallet para registrar en desempaque
    /// </summary>
    public class StorePalletRequestDto
    {
        public int IdPallet { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public bool? Claim { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para pallet registrado en desempaque
    /// </summary>
    public class StorePalletResponseDto
    {
        public int IdPallet { get; set; }
        public string PalletCode { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// DTO para distribución de un componente a una estación específica
    /// </summary>
    public class ComponentStationDistributionDto
    {
        public int IdStation { get; set; }
        public string StationName { get; set; } = null!;
        public int Quantity { get; set; }
        public int CantCar { get; set; }
    }

    /// <summary>
    /// DTO para distribución de un componente (BoxProductDetail) a múltiples estaciones
    /// </summary>
    public class ComponentDistributionDto
    {
        public int IdBoxProductDetail { get; set; }
        public int IdComponent { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentName { get; set; }
        public int ExpectedQuantity { get; set; }
        public int RevisedQuantity { get; set; }
        public List<ComponentStationDistributionDto> StationDistributions { get; set; } = new();
    }

    /// <summary>
    /// DTO de distribución de caja (agrupa componentes)
    /// </summary>
    public class BoxDistributionDetailDto
    {
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
        public string? BoxStatus { get; set; }
        public List<ComponentDistributionDto> ComponentDistributions { get; set; } = new();
    }

    /// <summary>
    /// DTO para formato de listado de desempaque
    /// Retorna pallets con cajas y distribución de componentes por estación
    /// </summary>
    public class UnpackingFormatDto
    {
        public int CkdLotId { get; set; }
        public string LotCode { get; set; } = null!;
        public string Product { get; set; } = null!;
        public List<PalletUnpackingDetailDto> Pallets { get; set; } = new();
    }

    /// <summary>
    /// DTO de detalle de pallet para desempaque
    /// </summary>
    public class PalletUnpackingDetailDto
    {
        public int IdPallet { get; set; }
        public string PalletCode { get; set; } = null!;
        public List<BoxDistributionDetailDto> Boxes { get; set; } = new();
    }

    /// <summary>
    /// DTO para solicitud de creación de registros en labeled_box
    /// Solo requiere el IdLot, el resto se genera automáticamente
    /// </summary>
    public class CreateLabeledBoxesRequestDto
    {
        public int CkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para labeled_box creado
    /// </summary>
    public class LabeledBoxCreatedResponseDto
    {
        public int IdLabeledBox { get; set; }
        public int IdBox { get; set; }
        public string BoxCode { get; set; } = null!;
        public int IdComponent { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentName { get; set; }
        public int IdStation { get; set; }
        public string StationName { get; set; } = null!;
        public int Quantity { get; set; }
        public string SupplyStatus { get; set; } = null!;
    }
}
