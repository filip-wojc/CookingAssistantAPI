using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO;

namespace CookingAssistantAPI.Repositories
{
    public interface IRepositoryRecipe
    {
        Task AddRecipeAsync(Recipe recipe);
        Task<int> SaveChangesAsync();
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task<Recipe> GetRecipeByNameAsync(string recipeName);
        Task<List<string>> GetAllNutrientsListAsync();
        Task<List<string>> GetAllIngredientsListAsync();
    }
}
