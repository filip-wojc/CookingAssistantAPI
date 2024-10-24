
using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Exceptions;
using CookingAssistantAPI.Services.UserServices;
using Microsoft.EntityFrameworkCore;

namespace CookingAssistantAPI.Repositories.Reviews
{
    public class RepositoryReview : IRepositoryReview
    {
        private readonly CookingDbContext _context;
        private readonly IUserContextService _userContext;
        public RepositoryReview(CookingDbContext context, IUserContextService userContext)
        {
            _context = context;
            _userContext = userContext;
        }
        public async Task AddReviewAsync(int recipeId, Review review)
        {
           var recipe = await GetRecipeById(recipeId);
           if (recipe.UsersReviews.Where(r => r.RatedRecipeId == recipeId && r.ReviewAuthorId == _userContext.UserId).Count() > 1)
           {
                throw new BadRequestException("You can't add more than one review to the recipe");
           }
           review.DateCreated = DateTime.UtcNow;
           _context.Reviews.Add(review);

            recipe.Ratings = recipe.UsersReviews.Average(r => (double)r.Value);
            recipe.VoteCount += 1;
        }

        public async Task ModifyReviewAsync(int recipeId, Review updatedReview)
        {
            var recipe = await GetRecipeById(recipeId);

            var review = recipe.UsersReviews.FirstOrDefault(r => r.RatedRecipeId == recipeId && r.ReviewAuthorId == _userContext.UserId);

            if (review == null)
            {
                throw new NotFoundException("Review not found for the current user.");
            }

            review.Value = updatedReview.Value;
            review.Description = updatedReview.Description;
            review.DateModified = DateTime.UtcNow;

            _context.Reviews.Update(review);
        }
        // Fetch user's review fields
        public async Task<Review> GetUserReview(int recipeId)
        {
            var recipe = await GetRecipeById(recipeId);
            var review = recipe.UsersReviews.FirstOrDefault(r => r.RatedRecipeId == recipeId && r.ReviewAuthorId == _userContext.UserId);
            if (review == null)
            {
                throw new NotFoundException("Review not found for the current user.");
            }
            return review;
        }
        
        /*
        // get limit number of reviews for a recipe, if not specified get all reviews
        public async Task<List<Review>> GetReviewsAsync(int recipeId, int? limit = null)
        {
            var recipe = await GetRecipeById(recipeId);

            // If limit is specified, ensure it doesn't exceed the total review count
            if (limit.HasValue && limit.Value > 0)
            {
                var totalReviews = recipe.UsersReviews.Count;

                // If the limit is greater than available reviews, return all reviews
                if (limit.Value > totalReviews)
                {
                    return recipe.UsersReviews.ToList();
                }

                // Otherwise, return the limited number of reviews
                return recipe.UsersReviews.Take(limit.Value).ToList();
            }

            // If no limit is specified, return all reviews
            return recipe.UsersReviews.ToList();
        }
        */
        

        private async Task<Review> GetReviewById(int reviewId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review is null)
            {
                throw new NotFoundException("Review not found");
            }

            return review;
        }

        private async Task<Recipe> GetRecipeById(int recipeId)
        {
            var recipe = await _context.Recipes.Include(r => r.UsersReviews).ThenInclude(r => r.ReviewAuthor)
                .FirstOrDefaultAsync(r => r.Id == recipeId);
            if (recipe is null)
            {
                throw new NotFoundException("Recipe not found");
            }
            return recipe;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
