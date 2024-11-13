using CookingAssistantAPI.Database.Models;

namespace CookingAssistantAPI.Repositories.Ingredients
{
    public interface IRepositoryResources
    {
        Task<List<string>> GetAllIngredientsListAsync();
        Task<List<string>> GetAllUnitsListAsync();
        Task<List<Category>> GetAllCategoriesListAsync();
        Task<List<Occasion>> GetAllOccasionsListAsync();
        Task<List<Difficulty>> GetAllDifficultiesListAsync();
    }
}
