using CookingAssistantAPI.Database.Models;

namespace CookingAssistantAPI.Repositories.Reviews
{
    public interface IRepositoryReview
    {
        Task AddReviewAsync(Review review, int recipeId);
        Task<int> SaveChangesAsync();
    }
}
