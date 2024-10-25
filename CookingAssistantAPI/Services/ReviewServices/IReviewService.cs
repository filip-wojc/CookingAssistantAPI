using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Reviews;

namespace CookingAssistantAPI.Services.ReviewServices
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(int recipeId, ReviewCreateDTO dto);
        Task<bool> ModifyReviewAsync(int recipeId, ReviewCreateDTO dto);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<ReviewGetDTO> GetUserReview(int recipeId);
        Task<List<ReviewGetDTO>> GetReviewsAsync(int recipeId);
        Task<byte[]> GetProfilePictureAsync(int reviewId);
    }
}
