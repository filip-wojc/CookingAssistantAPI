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
        Task<bool> UploadProfilePicture(int? userId, byte[] imageData);
        Task<bool> RemoveUserFromDbAsync(int? userId, string userName);
        Task<bool> RemoveRecipeFromFavouritesAsync(int?userId, int recipeId);
    }
}
