using CookingAssistantAPI.Database.Models;

namespace CookingAssistantAPI.Repositories.Reviews
{
    public interface IRepositoryReview
    {
        Task AddReviewAsync(int recipeId, Review review);
        Task ModifyReviewAsync(int recipeId, Review updatedReview);
        Task<Review> GetUserReview(int recipeId);
        //Task<List<Review>> GetReviewsAsync(int recipeId, int? limit = null);

        Task<int> SaveChangesAsync();
    }
}
