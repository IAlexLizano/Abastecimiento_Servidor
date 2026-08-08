using AuthApplication.DTOs.Stores;
using Shared.Application.Wrappers;

namespace AuthApplication.Interfaces
{
    public interface IStoresRepository
    {
        /// <summary>
        /// Obtiene todas las tiendas disponibles
        /// </summary>
        /// <returns>Lista de tiendas con Response wrapper</returns>
        Task<List<StoreResponseDto>> GetAllStoresAsync();
    }
}
