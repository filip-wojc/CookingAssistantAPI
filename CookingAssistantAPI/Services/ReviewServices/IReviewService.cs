using CookingAssistantAPI.DTO.Reviews;

namespace CookingAssistantAPI.Services.ReviewServices
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(ReviewCreateDTO dto, int recipeId);
    }
}
