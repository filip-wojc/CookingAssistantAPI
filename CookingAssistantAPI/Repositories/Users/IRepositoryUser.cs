using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Repositories.Users
{
    public interface IRepositoryUser
    {
        Task<bool> AddUserToDbAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> AddRecipeToFavourites(int recipeId, int? userId);
        Task<(List<Recipe>, int totalItems)> GetPaginatedFavouriteRecipesAsync(int? userId, RecipeQuery query);
        Task<bool> UploadProfilePicture(int? userId, byte[] imageData);
        Task<bool> RemoveUserFromDbAsync(int? userId);
        Task<bool> RemoveRecipeFromFavouritesAsync(int?userId, int recipeId);
        Task<byte[]> GetProfilePictureAsync(int? userId);
        Task<bool> ChangePasswordAsync(int? userId, UserPasswordChangeDTO dto);
        Task<bool> IsRecipeInFavouritesAsync(int? userId, int recipeId);
    }
}
