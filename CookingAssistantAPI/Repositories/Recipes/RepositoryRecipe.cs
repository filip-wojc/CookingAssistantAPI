using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CookingAssistantAPI.Repositories.Recipes
{
    public class RepositoryRecipe : IRepositoryRecipe
    {
        private CookingDbContext _context;
        public RepositoryRecipe(CookingDbContext context)
        {
            _context = context;
        }
        public async Task AddRecipeAsync(Recipe recipe)
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

        public async Task<List<string>> GetAllIngredientsListAsync()
        {

            var ingredients = await _context.Ingredients
                .Select(i => i.IngredientName)
                .ToListAsync();

            if (ingredients is null)
            {
                throw new NotFoundException("Ingredients not found");
            }

            return ingredients;
        }

        public async Task<List<string>> GetAllNutrientsListAsync()
        {

            var nutrients = await _context.Nutrients
                .Select(n => n.NutrientName)
                .ToListAsync();

            if (nutrients is null)
            {
                throw new NotFoundException("Nutrients not found");
            }

            return nutrients;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // USER VALIDATION BEFORE REMOVING RECIPE
        /*
        public async Task<bool> DeleteRecipeByIdAsync(int recipeId, int userId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if(recipe == null)
            {
                throw new NotFoundException("Recipe to delete not found");
            }

            _context.Recipes.Remove(recipe);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        */
    }

}