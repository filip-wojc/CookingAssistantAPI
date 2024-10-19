using CookingAssistantAPI.DTO.Users;

namespace CookingAssistantAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<bool> RegisterUser(UserRegisterDTO dto);
        Task<string> GenerateToken(UserLoginDTO dto);
        Task<bool> AddRecipeToFavourites(int recipeId);
    }
}
