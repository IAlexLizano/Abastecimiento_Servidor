namespace PreOpeningApplication.DTOs
{
    /// <summary>
    /// DTO de respuesta para información de receta del vehículo
    /// Contiene datos técnicos del modelo, versión y año del vehículo
    /// </summary>
    public class VehicleRecipeResponseDto
    {
        public string Model { get; set; } = null!;
        public string? Version { get; set; }
        public int? Year { get; set; }
        public string CpaCode { get; set; } = null!;
        public string ModelPrefix { get; set; } = null!;
    }

    /// <summary>
    /// DTO de solicitud para obtener información de receta del vehículo
    /// </summary>
    public class GetVehicleRecipeRequestDto
    {
        public int CkdLotId { get; set; }
    }

    /// <summary>
    /// DTO de solicitud para extraer prefijo de modelo del código CKD
    /// </summary>
    public class ExtractModelPrefixRequestDto
    {
        public string CkdCode { get; set; } = null!;
    }

    /// <summary>
    /// DTO de solicitud para obtener última apertura del mismo modelo
    /// </summary>
    public class GetLastOpeningOfModelRequestDto
    {
        public string ModelPrefix { get; set; } = null!;
    }

    /// <summary>
    /// DTO de respuesta simple para prefijo de modelo
    /// </summary>
    public class ModelPrefixResponseDto
    {
        public string ModelPrefix { get; set; } = null!;
        public string OriginalCkdCode { get; set; } = null!;
    }
}
