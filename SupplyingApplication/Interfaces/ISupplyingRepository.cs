using SupplyingApplication.DTOs;

namespace SupplyingApplication.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Abastecimiento (Supplying).
    /// 
    /// Define contratos para:
    /// - Transferencias de cartones (Cardboard) a estaciones de trabajo
    /// - Registro de envíos y recepciones de componentes
    /// 
    /// Todos los métodos retornan DTOs para mantener separación entre
    /// la capa de datos y la capa de presentación.
    /// </summary>
    public interface ISupplyingRepository
    {
        /// <summary>
        /// Abastece una estación registrando el envío de cartones (Cardboard).
        /// Actualiza send_user y send_date, y realiza cascada de estados.
        /// Cardboard → ABASTECIDO_PARCIAL, Supply → EN_PROGRESO
        /// </summary>
        /// <param name="idSupply">ID del supply a abastecer</param>
        /// <param name="idUserSend">ID del usuario que realiza el envío</param>
        /// <returns>DTO con información del supply actualizado</returns>
        Task<SupplyDto> SupplyStationAsync(int idSupply, int idUserSend);

        /// <summary>
        /// Obtiene cartones (Cardboard) pendientes de abastecimiento.
        /// Solo devuelve aquellos con estado PENDIENTE en Supply.
        /// Incluye información del componente y código de pallet.
        /// </summary>
        /// <returns>Lista de cartones con detalles completos</returns>
        Task<BoxListResponseDto> GetPendingBoxesAsync();

        /// <summary>
        /// Recibe un cartón (Cardboard) registrando la recepción.
        /// Actualiza receive_user y receive_date, y realiza cascada de estados.
        /// Cardboard → ABASTECIDO, Supply → COMPLETADO
        /// </summary>
        /// <param name="idSupply">ID del supply a recibir</param>
        /// <param name="idUserReceive">ID del usuario que recibe</param>
        /// <returns>DTO con información del supply actualizado</returns>
        Task<SupplyDto> ReceiveBoxAsync(int idSupply, int idUserReceive);

        /// <summary>
        /// Obtiene supplies en estado EN_PROCESO con todos sus detalles.
        /// </summary>
        /// <returns>Lista de supplies en proceso con detalles completos</returns>
        Task<BoxListResponseDto> GetSuppliesInProgressAsync();

        /// <summary>
        /// Obtiene todos los cartones con sus detalles, sin importar el estado.
        /// </summary>
        /// <returns>Lista completa de cartones con todos los detalles</returns>
        Task<BoxListResponseDto> GetAllBoxesAsync();
    }
}

