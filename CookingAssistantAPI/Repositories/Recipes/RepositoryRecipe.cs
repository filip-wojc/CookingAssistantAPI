using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
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


        public async Task<bool> ModifyRecipeAsync(Recipe recipe, int recipeId, int? userId)
        {
            var recipeToModify = await GetRecipeById(recipeId);

            if (recipeToModify.CreatedById != userId)
            {
                throw new ForbidException("You can't modify a recipe which was not added by you");
            }

            recipeToModify.ImageData = recipe.ImageData;
            recipeToModify.Steps = recipe.Steps;
            recipeToModify.Serves = recipe.Serves;
            recipeToModify.Description = recipe.Description;
            recipeToModify.Name = recipe.Name;
            recipeToModify.Difficulty = recipe.Difficulty;
            recipeToModify.TimeInMinutes = recipe.TimeInMinutes;
            recipeToModify.RecipeIngredients = recipe.RecipeIngredients;
            recipeToModify.RecipeNutrients = recipe.RecipeNutrients;

            var result = await _context.SaveChangesAsync();

            return result > 0;

        }

        public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
        {
            return await GetRecipeById(recipeId);
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

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            var recipes = await _context.Recipes.Include(r => r.Category)
                .Include(r => r.CreatedBy).ToListAsync();
            return recipes;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRecipeByIdAsync(int recipeId, int? userId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

            if(recipe == null)
            {
                throw new NotFoundException("Recipe to delete not found");
            }

            if(recipe.CreatedById != userId)
            {
                throw new ForbidException("You can't delete a recipe which was not added by you");
            }

            _context.Recipes.Remove(recipe);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<byte[]?> GetRecipeImageAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
            if (recipe == null)
            {
                throw new NotFoundException("Recipe not found");
            }

            return recipe.ImageData;
        }

        private async Task<Recipe> GetRecipeById(int recipeId)
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
       
    }

}