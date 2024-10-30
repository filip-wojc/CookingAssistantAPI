using CookingAssistantAPI.Database.Models;

namespace CookingAssistantAPI.Repositories.Reviews
{
    public interface IRepositoryReview
    {
        Task AddReviewAsync(int recipeId, int?userId, Review review);
        Task ModifyReviewAsync(int recipeId, int? userId,Review updatedReview);
        Task<Review> GetUserReview(int recipeId, int? userId);
        Task<byte[]> GetProfilePictureAsync(int reviewId);
        Task<bool> DeleteReviewAsync(int recipeId, int? userId);
        Task<List<Review>> GetReviewsAsync(int recipeId);
        Task<int> SaveChangesAsync();
    }
}
