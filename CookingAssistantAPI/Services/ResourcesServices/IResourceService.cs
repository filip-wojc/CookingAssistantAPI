using CookingAssistantAPI.DTO.Resources;

namespace CookingAssistantAPI.Services.IngredientServices
{
    public interface IResourceService
    {
        Task<List<string>> GetAllIngredientsListAsync();
        Task<List<DifficultiesGetDTO>> GetAllDifficultiesAsync();
        Task<List<OccasionsGetDTO>> GetAllOccasionsAsync();
        Task<List<CategoriesGetDTO>> GetAllCategoriesAsync();
    }
}
