using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;

namespace CookingAssistantAPI.Services
{
    public interface IRecipeService
    {
        Task<bool> AddRecipe(RecipeCreateDTO recipeDto);
        Task<RecipeGetDTO> GetRecipeByIdAsync(int recipeId);
        Task<RecipeGetDTO> GetRecipeByNameAsync(string recipeName);
        Task<List<string>> GetAllNutrientsAsync();
        Task<List<string>> GetAllIngredientsAsync();
    }
}
