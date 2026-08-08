using OpeningApplication.DTOs.Opening;

namespace OpeningApplication.Interfaces
{
    public interface IBoxRepository
    {
        #region Box Query Operations
        /// <summary>Obtiene cajas de un pallet con todos sus detalles incluyendo labeled_boxes</summary>
        Task<IEnumerable<BoxWithLabeledBoxesResponseDto>> GetBoxesByPalletAsync(int palletId);

        /// <summary>Obtiene la última caja asignada a un usuario (solo en proceso) con todos sus detalles</summary>
        Task<BoxWithLabeledBoxesResponseDto> GetBoxesByUserAsync(int userId);

        /// <summary>Obtiene todas las cajas asignadas a un usuario sin importar su estado con todos sus detalles</summary>
        Task<IEnumerable<BoxWithLabeledBoxesResponseDto>> GetAllBoxesByUserAsync(int userId);

        /// <summary>Obtiene una caja específica por ID con todos sus detalles incluyendo labeled_boxes</summary>
        Task<BoxWithLabeledBoxesResponseDto> GetBoxAsync(int boxId);

        /// <summary>Obtiene todas las cajas procesadas del día actual asignadas a un usuario, sin detalles de labeled_boxes</summary>
        Task<IEnumerable<ProcessedBoxTodayResponseDto>> GetProcessedBoxesTodayByUserAsync(int userId);
        #endregion

        #region Box Operation Commands
        /// <summary>Registra el desempaque de un cartón de caja (unboxing)</summary>
        Task<UnboxingRegisteredResponseDto> RegisterUnboxingAsync(RegisterUnboxingRequestDto request);

        /// <summary>Registra la apertura de caja con cantidades verificadas</summary>
        Task<BoxOpeningRegisteredResponseDto> RegisterBoxOpeningAsync(RegisterBoxOpeningRequestDto request);
        #endregion
    }
}