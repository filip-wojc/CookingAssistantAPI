
using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace CookingAssistantAPI.Repositories.Reviews
{
    public class RepositoryReview : IRepositoryReview
    {
        private readonly CookingDbContext _context;
        public RepositoryReview(CookingDbContext context)
        {
            _context = context;
        }
        public async Task AddReviewAsync(int recipeId, int? userId, Review review)
        {
           var recipe = await GetRecipeById(recipeId);
           if (recipe.UsersReviews.Where(r => r.RatedRecipeId == recipeId && r.ReviewAuthorId == userId).Count() >= 1)
           {
                throw new BadRequestException("You can't add more than one review to the recipe");
           }
           if (recipe.CreatedById == userId)
           {
                throw new ForbidException("You can't review your own recipe");
           }
           
           review.DateCreated = DateTime.UtcNow;
           _context.Reviews.Add(review);

           recipe.Ratings = recipe.UsersReviews.Average(r => (double)r.Value);
           recipe.VoteCount += 1;
        }

        public async Task ModifyReviewAsync(int recipeId, int? userId, Review updatedReview)
        {
            var recipe = await GetRecipeById(recipeId);

            var review = recipe.UsersReviews.FirstOrDefault(r => r.RatedRecipeId == recipeId && r.ReviewAuthorId == userId);

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
        public async Task<Review> GetUserReview(int recipeId, int? userId)
        {
            var recipe = await GetRecipeById(recipeId);
            var review = recipe.UsersReviews.FirstOrDefault(r => r.RatedRecipeId == recipeId && r.ReviewAuthorId == userId);
            if (review == null)
            {
                throw new NotFoundException("Review not found for the current user.");
            }
            return review;
        }
        
        
        public async Task<List<Review>> GetReviewsAsync(int recipeId)
        {
            var recipe = await GetRecipeById(recipeId);

            var userReviews = recipe.UsersReviews.ToList();
            if (userReviews.IsNullOrEmpty())
            {
                throw new NotFoundException("Reviews not found for this recipe");
            }

            return recipe.UsersReviews.ToList();
        }
        

        public async Task<bool> DeleteReviewAsync(int recipeId, int? userId)
        {
            var recipe = await GetRecipeById(recipeId);
            var review = recipe.UsersReviews.FirstOrDefault(r => r.RatedRecipeId == recipeId && r.ReviewAuthorId == userId);
            if (review == null)
            {
                throw new NotFoundException("Review not found for the current user.");
            }
            if (review.ReviewAuthorId != userId)
            {
                throw new ForbidException("You can only delete your own reviews");
            }
            _context.Reviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
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
        public async Task<byte[]> GetProfilePictureAsync(int reviewId)
        {
            var review = await _context.Reviews.Include(r => r.ReviewAuthor).FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review is null)
            {
                throw new NotFoundException("Review not found");
            }
            var profilePic = review.ReviewAuthor!.ProfilePictureImageData;
            if (profilePic is null)
            {
                throw new NotFoundException("Profile picture for review not found");
            }
            return profilePic;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

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


    }
}
