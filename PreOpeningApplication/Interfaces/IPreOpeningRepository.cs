using PreOpeningApplication.DTOs;

namespace PreOpeningApplication.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Pre-Apertura
    /// </summary>
    public interface IPreOpeningRepository
    {
        #region Lot Management Operations
        /// <summary>
        /// Carga un lote completo con toda su estructura (contenedores, pallets, cajas, detalles)
        /// </summary>
        Task<string> LoadLotAsync(LoadCkdLotRequestDto request);

        /// <summary>
        /// Obtiene un lote completo con todos sus detalles anidados
        /// </summary>
        Task<CkdLotFullDetailsDto> GetCkdLotWithAllDetailsAsync(int ckdLotId);

        /// <summary>
        /// Pone contenedores en bodega y actualiza estado en cascada
        /// </summary>
        Task<string> StoreContainerAsync(StoreContainerRequestDto request);
        #endregion

        #region Format Generation Operations
        /// <summary>
        /// Genera formato de descarga de contenedores (solo bodega ESTACION)
        /// </summary>
        Task<ContainerUnloadingFormatDto> GenerateContainerUnloadingFormatAsync(int ckdLotId);

        /// <summary>
        /// Genera formato de listado de desempaque con distribución por estación
        /// </summary>
        Task<UnpackingFormatDto> GenerateUnpackingFormatAsync(int ckdLotId);
        #endregion

        #region Unpacking Operations
        /// <summary>
        /// Registra un pallet en desempaque y actualiza estado en cascada
        /// </summary>
        Task<string> StorePalletAsync(StorePalletRequestDto request);

        /// <summary>
        /// Crea registros en tabla labeled_box para desempaque
        /// </summary>
        Task<string> CreateCardboardsAsync(CreateLabeledBoxesRequestDto request);
        #endregion

        #region Query Operations
        /// <summary>
        /// Obtiene todos los pallets de un contenedor específico
        /// </summary>
        Task<PalletsByContainerResponseDto> GetPalletsByContainerAsync(int idContainer);

        /// <summary>
        /// Obtiene todos los contenedores con sus pallets de un CKD Lot
        /// </summary>
        Task<ContainersWithPalletsByCkdResponseDto> GetContainersWithPalletsByCkdAsync(int idCkdLot);

        /// <summary>
        /// Obtiene lista simplificada de todos los lotes CKD
        /// </summary>
        Task<CkdLotSimpleListResponseDto> GetAllCkdLotsAsync(string? status = null);

        /// <summary>
        /// Obtiene lista simplificada de todos los contenedores de un lote CKD
        /// </summary>
        Task<ContainerSimpleListResponseDto> GetAllContainersByCkdLotAsync(int idCkdLot);

        /// <summary>
        /// Obtiene lista simplificada de todos los pallets de un contenedor
        /// </summary>
        Task<PalletSimpleListResponseDto> GetAllPalletsByContainerAsync(int idContainer);
        #endregion
    }
}
