using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Repositories
{
    public interface IRepositoryRecipe
    {
        Task AddRecipeAsync(Recipe recipe);
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task<Recipe> GetRecipeByNameAsync(string recipeName);
        Task<(List<Recipe>, int totalItems)> GetPaginatedRecipesAsync(RecipeQuery query);
        Task<List<Recipe>> GetAllRecipesAsync();
        Task<byte[]?> GetRecipeImageAsync(int recipeId);
        Task<int> SaveChangesAsync();
        Task<bool> DeleteRecipeByIdAsync(int recipeId, int? userId);
        Task<bool> ModifyRecipeAsync(Recipe recipe, int recipeId, int? userId);
    }
}