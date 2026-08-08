using ComponentsApplication.DTOs;

namespace ComponentsApplication.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Novedades
    /// </summary>
    public interface IComponentsRepository
    {
        Task<IEnumerable<ComponentResponseDto>> GetAllComponents();

        Task<string> LoadRecipe(string modelCode, List<LoadRecipeRequestDto> request);
        Task<string> CreateComponent(CreateComponentRequestDto request);

        Task<string> UpdateComponent(UpdateComponentRequestDto request);

        Task<string> DeleteComponent(int id);

        Task<ComponentResponseDto> GetComponentById(int id);

        Task<IEnumerable<RecipeResponseDto>> GetRecipesByModel(int idModel);
    }
}
