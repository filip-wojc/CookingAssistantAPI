﻿using CookingAssistantAPI.DTO;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<bool> RegisterUser(UserRegisterDTO dto);
        Task<LogInResponseDTO> GenerateToken(UserLoginDTO dto);
        Task<bool> AddRecipeToFavourites(int recipeId);
        Task<PageResult<RecipeSimpleGetDTO>> GetFavouriteRecipesAsync(RecipeQuery query);
        Task<PageResult<RecipeSimpleGetDTO>> GetUserRecipesAsync(RecipeQuery query);
        Task<bool> UploadProfilePicture(UploadFileDTO profilePicture);
        Task<bool> DeleteUserAsync(PasswordDTO password);
        Task<bool> RemoveRecipeFromFavouritesAsync(int recipeId);
        Task<byte[]> GetUserProfilePictureAsync();
        Task<bool> ChangeUserPassword(UserPasswordChangeDTO dto);
        Task<bool> IsRecipeInFavouritesAsync(int recipeId);
        Task<bool> IsRecipeCreatedByUserAsync(int recipeId);
    }
}
