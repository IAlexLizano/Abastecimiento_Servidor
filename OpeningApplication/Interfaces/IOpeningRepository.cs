using OpeningApplication.DTOs.Opening;

namespace OpeningApplication.Interfaces
{
    public interface IOpeningRepository
    {
        /// <summary>
        /// Obtiene información básica (id y código) de lotes, contenedores, pallets y cajas 
        /// que están en estado "EN_PROCESO"
        /// </summary>
        /// <returns>DTO con listas de información básica de lotes, contenedores, pallets y cajas</returns>
        Task<IEnumerable<BasicInformationResponseDto>> GetBasicInformationInProcessAsync();
    }
}
