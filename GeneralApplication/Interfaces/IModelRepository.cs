using GeneralApplication.DTOs;

namespace GeneralApplication.Interfaces
{
    public interface IModelRepository
    {
        Task<IEnumerable<ModelDto>> GetAllModels();
    }
}
