using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO;

namespace CookingAssistantAPI.Services
{
    public interface IRecipeService
    {
        Task<bool> AddRecipe(RecipeCreateDTO recipeDto);
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task<Recipe> GetRecipeByNameAsync(string recipeName);
        Task<List<string>> GetAllNutrientsAsync();
        Task<List<string>> GetAllIngredientsAsync();
    }
}
