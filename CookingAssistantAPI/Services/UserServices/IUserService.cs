using CookingAssistantAPI.DTO;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<bool> RegisterUser(UserRegisterDTO dto);
        Task<string> GenerateToken(UserLoginDTO dto);
        Task<bool> AddRecipeToFavourites(int recipeId);
        Task<List<RecipeSimpleGetDTO>> GetFavouriteRecipesAsync(RecipeQuery query);
        Task<bool> UploadProfilePicture(UploadFileDTO profilePicture);
    }
}
