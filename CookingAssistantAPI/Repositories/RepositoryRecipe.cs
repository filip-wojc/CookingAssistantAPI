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
            // Check and attach existing ingredients
            foreach (var recipeIngredient in recipe.RecipeIngredients)
            {
                var existingIngredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.IngredientName == recipeIngredient.Ingredient.IngredientName);

                if (existingIngredient != null)
                {
                    recipeIngredient.Ingredient = existingIngredient;
                }
                else
                {
                    // EF Core will track this as a new entity
                    _context.Ingredients.Add(recipeIngredient.Ingredient);
                }
            }

            // Check and attach existing nutrients
            foreach (var recipeNutrient in recipe.RecipeNutrients)
            {
                var existingNutrient = await _context.Nutrients
                    .FirstOrDefaultAsync(n => n.NutrientName == recipeNutrient.Nutrient.NutrientName);

                if (existingNutrient != null)
                {
                    recipeNutrient.Nutrient = existingNutrient;
                }
                else
                {
                    // EF Core will track this as a new entity
                    _context.Nutrients.Add(recipeNutrient.Nutrient);
                }
            }

            // Now add the recipe
            await _context.Recipes.AddAsync(recipe);
            int success = await _context.SaveChangesAsync();

            return success > 0;
        }

        public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.CreatedBy)
                .Include(r => r.Steps)
                .Include(r => r.RecipeIngredients) // Include RecipeIngredients
                    .ThenInclude(ri => ri.Ingredient) // Then include the related Ingredient
                .Include(r => r.RecipeNutrients) // Include RecipeNutrients
                    .ThenInclude(rn => rn.Nutrient) // Then include the related Nutrient
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe is null)
            {
                throw new NotFoundException("Recipe not found");
            }

            return recipe;
        }

        public async Task<Recipe> GetRecipeByNameAsync(string recipeName)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.CreatedBy)
                .Include(r => r.Steps)
                .Include(r => r.RecipeIngredients) // Include RecipeIngredients
                    .ThenInclude(ri => ri.Ingredient) // Then include the related Ingredient
                .Include(r => r.RecipeNutrients) // Include RecipeNutrients
                    .ThenInclude(rn => rn.Nutrient) // Then include the related Nutrient
                .FirstOrDefaultAsync(r => r.Name == recipeName);

            if (recipe is null)
            {
                throw new NotFoundException("Recipe not found");
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