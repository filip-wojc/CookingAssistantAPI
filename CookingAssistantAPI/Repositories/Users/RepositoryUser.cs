using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.DTO.Users;
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
            if (user.FavouriteRecipes.Contains(recipe))
            {
                throw new ForbidException("You can't add the same recipe to favourites");
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

        public async Task<bool> ChangePasswordAsync(int? userId, UserPasswordChangeDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.PasswordHash = dto.NewPassword;
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Recipe>> GetFavouriteRecipesAsync(int? userId)
        {
            var user = await _context.Users.Include(u => u.FavouriteRecipes).ThenInclude(r => r.Category).FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }
            return user.FavouriteRecipes.ToList();
        }

        public async Task<byte[]> GetProfilePictureAsync(int? userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var profilePicture = user.ProfilePictureImageData;
            if (profilePicture is null)
            {
                throw new BadRequestException("User does not have a profile picture");
            }
            return profilePicture;
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

        public async Task<bool> RemoveRecipeFromFavouritesAsync(int?userId, int recipeId)
        {
            var user = await _context.Users.Include(u => u.FavouriteRecipes).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
            if (recipe == null)
            {
                throw new NotFoundException("Recipe not found");
            }
            if (!user.FavouriteRecipes.Contains(recipe))
            {
                throw new BadRequestException("This recipe is not in your favourite recipes list");
            }
            user.FavouriteRecipes.Remove(recipe);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> RemoveUserFromDbAsync(int? userId, string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User to delete not found");
            }
            if (user.UserName != userName)
            {
                throw new ForbidException("Write your username before deleting an account");
            }
            var result = await _context.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();

            return result > 0;
        }

        public async Task<bool> UploadProfilePicture(int? userId, byte[] imageData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            user.ProfilePictureImageData = imageData;
            if(await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
           
        }
       
    }
}
