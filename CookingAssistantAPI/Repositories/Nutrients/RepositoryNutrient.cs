using CookingAssistantAPI.Database;
using CookingAssistantAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CookingAssistantAPI.Repositories.Nutrients
{
    public class RepositoryNutrient : IRepositoryNutrient
    {
        private readonly CookingDbContext _context;
        public RepositoryNutrient(CookingDbContext context)
        {
            _context = context;
        }
        public async Task<List<string>> GetAllNutrientsListAsync()
        {

            var nutrients = await _context.Nutrients
                .Select(n => n.NutrientName)
                .ToListAsync();

            if (nutrients.IsNullOrEmpty())
            {
                throw new NotFoundException("Nutrients not found");
            }

            return nutrients;
        }

    }
}
