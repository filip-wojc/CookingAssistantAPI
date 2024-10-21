using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;

namespace CookingAssistantAPI.Repositories.Users
{
    public interface IRepositoryUser
    {
        Task<bool> AddUserToDbAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> AddRecipeToFavourites(int recipeId, int? userId);
        Task<List<Recipe>> GetFavouriteRecipesAsync(int? userId);
        // Task<bool> RemoveUserFromDbAsync(int userId); FIX BEFORE USING
    }
}
