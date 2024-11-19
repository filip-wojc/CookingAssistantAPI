using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CookingAssistantAPI.Repositories.Ingredients
{
    public class RepositoryResources : IRepositoryResources
    {
        private readonly CookingDbContext _context;
        public RepositoryResources(CookingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories.IsNullOrEmpty())
            {
                throw new NotFoundException("Categories not found");
            }
            return categories;
        }

        public async Task<List<Difficulty>> GetAllDifficultiesListAsync()
        {
            var difficulties = await _context.Difficulties.ToListAsync();
            if (difficulties.IsNullOrEmpty())
            {
                throw new NotFoundException("Difficulties not found");
            }
            return difficulties;
        }

        public async Task<List<string>> GetAllIngredientsListAsync()
        {

            var ingredients = await _context.Ingredients
                .Select(i => i.IngredientName)
                .ToListAsync();

            if (ingredients.IsNullOrEmpty())
            {
                throw new NotFoundException("Ingredients not found");
            }

            return ingredients;
        }

        public async Task<List<Occasion>> GetAllOccasionsListAsync()
        {
            var occasions = await _context.Occasions.ToListAsync();
            if (occasions.IsNullOrEmpty())
            {
                throw new NotFoundException("Occasions not found");
            }
            return occasions;
        }

        public async Task<List<string>> GetAllUnitsListAsync()
        {
            var recipeIngredients = await _context.RecipeIngredients.ToListAsync();
            var units = recipeIngredients.Select(r => r.Unit).ToHashSet().ToList();
            if (units.IsNullOrEmpty())
            {
                throw new NotFoundException("Units not found");
            }
            return units;
        }
    }
}
