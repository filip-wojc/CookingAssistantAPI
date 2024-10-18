
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
        public async Task AddReviewAsync(Review review, int recipeId)
        {
           var recipe = await GetRecipeById(recipeId);
           if (recipe.UsersReviews.Where(r => r.RatedRecipeId == recipeId && r.ReviewAuthorId == _userContext.UserId).Count() > 1)
           {
                throw new BadRequestException("You can't add more than one review to the recipe");
           }

           _context.Reviews.Add(review);

            recipe.Ratings = recipe.UsersReviews.Average(r => (double)r.Value);
            recipe.VoteCount += 1;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private async Task<Recipe> GetRecipeById(int recipeId)
        {
            var recipe = await _context.Recipes.Include(r => r.UsersReviews)
                .FirstOrDefaultAsync(r => r.Id == recipeId);
            if (recipe is null)
            {
                throw new NotFoundException("Recipe not found");
            }
            return recipe;
        }
    }
}
