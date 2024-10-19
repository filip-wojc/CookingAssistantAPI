using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CookingAssistantAPI.Repositories.Users
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly CookingDbContext _context;
        public RepositoryUser(CookingDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRecipeToFavourites(int recipeId, int? userId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
            if (recipe is null)
            {
                throw new NotFoundException("Recipe not found");
            }
            var user = await _context.Users.Include(u => u.FavouriteRecipes).FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }
            user.FavouriteRecipes?.Add(recipe);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddUserToDbAsync(User user)
        {
            await _context.Users.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new BadRequestException("Invalid email address");
            }
            return user;
        }
        // NEEDS TO BE VALIDATED BEFORE USING
        /*
        public async Task<bool> RemoveUserFromDbAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User to delete not found");
            }

            _context.Users.Remove(user);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        */
    }
}
