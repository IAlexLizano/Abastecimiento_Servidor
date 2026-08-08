using GeneralApplication.DTOs;

namespace GeneralApplication.Interfaces
{
    public interface IStoreRepository
    {
        Task<IEnumerable<StoreDto>> GetAllStores();
    }
}
