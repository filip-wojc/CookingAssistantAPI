using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Exceptions;
using CookingAssistantAPI.Tools;
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
            recipeToModify.ModificationDate = DateTime.Now;

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
            
            recipeToModify.RecipeIngredients = recipe.RecipeIngredients;
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
                .Include(r => r.Difficulty)
                .Include(r => r.Occasion)
                .Include(r => r.CreatedBy)
                .Include(r => r.Steps)
                .Include(r => r.RecipeIngredients) // Include RecipeIngredients
                    .ThenInclude(ri => ri.Ingredient) // Then include the related Ingredient
                .FirstOrDefaultAsync(r => r.Name == recipeName);

            if (recipe is null)
            {
                throw new NotFoundException("Recipe not found");
            }

            return recipe;
        }

        public async Task<(List<Recipe>, int totalItems)> GetPaginatedRecipesAsync(RecipeQuery query)
        {
            var recipesQuery = _context.Recipes
            .Include(r => r.Category)
            .Include(r => r.Difficulty)
            .Include(r => r.Occasion)
            .Include(r => r.CreatedBy)
            .Include(r => r.RecipeIngredients).ThenInclude(i => i.Ingredient)
            .AsQueryable();

            // Filtrowanie
            recipesQuery = RecipeQueryProcessing.Filter(recipesQuery, query);

            // Wyszukiwanie
            recipesQuery = RecipeQueryProcessing.Search(recipesQuery, query);

            // Sortowanie
            recipesQuery = RecipeQueryProcessing.Sort(recipesQuery, query);

            int totalItems = recipesQuery.Count();

            // Paginacja
            if (query.PageNumber.HasValue && query.PageSize.HasValue)
            {
                recipesQuery = recipesQuery
                .Skip((query.PageNumber.Value - 1) * query.PageSize.Value)
                    .Take(query.PageSize.Value);
            }
            
            return (await recipesQuery.ToListAsync(), totalItems);
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            var recipes = await _context.Recipes.ToListAsync();
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
                .Include(r => r.Difficulty)
                .Include(r => r.Occasion)
                .Include(r => r.CreatedBy)
                .Include(r => r.Steps)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe is null)
            {
                throw new NotFoundException("Recipe not found");
            }

            return recipe;
        }

        public async Task<Recipe> GetRandomRecipePerDyAsync()
        {
            var currentDate = DateTime.UtcNow.Date;
            var currentRecipeIds = await _context.Recipes.Select(r => r.Id).ToListAsync();

            if (!currentRecipeIds.Any())
            {
                throw new NotFoundException("No recipes found in database");
            }

            int hash = currentDate.GetHashCode();
            int index = Math.Abs(hash) % currentRecipeIds.Count;

            var randomId = currentRecipeIds[index];

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Difficulty)
                .Include(r => r.Occasion)
                .Include(r => r.CreatedBy)
                .Include(r => r.Steps)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == randomId);

            if (recipe is null)
            {
                throw new NotFoundException("Recipe not found");
            }

            return recipe;
        }
    }

}