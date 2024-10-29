using CookingAssistantAPI.Database;
using CookingAssistantAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CookingAssistantAPI.Repositories.Ingredients
{
    public class RepositoryIngredient : IRepositoryIngredient
    {
        private readonly CookingDbContext _context;
        public RepositoryIngredient(CookingDbContext context)
        {
            _context = context;
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
    }
}
