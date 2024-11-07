using CookingAssistantAPI.Database;
using CookingAssistantAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

            if (ingredients.IsNullOrEmpty())
            {
                throw new NotFoundException("Ingredients not found");
            }

            return ingredients;
        }
    }
}
