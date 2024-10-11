using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CookingAssistantAPI.Repositories
{
    public class RepositoryRecipe : IRepositoryRecipe
    {
        private CookingDbContext _context;
        public RepositoryRecipe(CookingDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddRecipeAsync(Recipe recipe)
        {
            int  success = 0;
            await _context.AddAsync(recipe);
            success = await _context.SaveChangesAsync();

            return success > 0;
        }

        public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
        {
            var recipe = await _context.Recipes.Include(r => r.Nutrients).Include(r => r.Ingredients).Include(r => r.Category)
                .Include(r => r.CreatedBy).Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe is null)
            {
                throw new NotFoundException("recipe not found");
            }

            return recipe;
        }

        public async Task<Recipe> GetRecipeByNameAsync(string recipeName)
        {
            var recipe = await _context.Recipes.Include(r => r.Nutrients).Include(r => r.Ingredients).Include(r => r.Category)
                .Include(r => r.CreatedBy).Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Name == recipeName);

            if (recipe is null)
            {
                throw new NotFoundException("recipe not found");
            }

            return recipe;
        }

        /*
        public async Task<bool> DeleteRecipeAsync(Recipe recipe)
        {
        int success = 0;
        await _context
        success = await _context.SaveChangesAsync();

        return success > 0;
        }
        */
    }

}