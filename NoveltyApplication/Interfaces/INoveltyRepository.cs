using NoveltyApplication.DTOs;

namespace NoveltyApplication.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Novedades (Engineering Issues)
    /// Maneja registro, seguimiento y resolución de fallos encontrados en componentes
    /// </summary>
    public interface INoveltyRepository
    {
        #region Novelty Registration
        /// <summary>Registra una novedad (fallo) encontrado en un componente</summary>
        Task<EngineeringIssueDto> RegisterNoveltyAsync(RegisterNoveltyRequestDto request);

        /// <summary>Obtiene todas las novedades de una caja específica</summary>
        Task<IEnumerable<EngineeringIssueDto>> GetNoveltyByBoxAsync(int boxId);

        /// <summary>Obtiene todas las novedades de un pallet específico</summary>
        Task<IEnumerable<EngineeringIssueDto>> GetNoveltyByPalletAsync(int palletId);

        /// <summary>Obtiene una novedad específica por ID</summary>
        Task<EngineeringIssueDto> GetNoveltyByIdAsync(int issueId);
        #endregion

        #region Novelty Status Operations
        /// <summary>Resuelve una novedad (cambia estado a RESUELTA)</summary>
        Task<string> ResolveNoveltyAsync(int issueId);

        /// <summary>Acepta una novedad (cambia estado a EN_PROGRESO)</summary>
        Task<string> AcceptNoveltyAsync(int issueId);

        /// <summary>Obtiene todas las novedades abiertas (no resueltas)</summary>
        Task<IEnumerable<EngineeringIssueDto>> GetOpenNovelties();

        /// <summary>Obtiene novedades reportadas por un usuario específico</summary>
        Task<IEnumerable<EngineeringIssueDto>> GetNoveltyByUserAsync(int userId);
        #endregion
    }
}
