using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Exceptions;
using CookingAssistantAPI.Tools;
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
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddUserToDbAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePasswordAsync(int? userId, UserPasswordChangeDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.PasswordHash = dto.NewPassword;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<(List<Recipe>, int totalItems)> GetPaginatedFavouriteRecipesAsync(int? userId, RecipeQuery query)
        {
            var user = await _context.Users.Include(u => u.FavouriteRecipes).FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var favouriteRecipesIds = user.FavouriteRecipes.Select(u => u.Id).ToList();

            var recipesQuery = _context.Recipes
           .Include(r => r.Category)
           .Include(r => r.Difficulty)
           .Include(r => r.Occasion)
           .Include(r => r.CreatedBy)
           .Include(r => r.RecipeIngredients).ThenInclude(i => i.Ingredient)
           .Where(r => favouriteRecipesIds.Contains(r.Id))
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

        public async Task<bool> IsRecipeInFavouritesAsync(int? userId, int recipeId)
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
            return user.FavouriteRecipes.Contains(recipe);
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

        public async Task<bool> RemoveUserFromDbAsync(int? userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User to delete not found");
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
            return await _context.SaveChangesAsync() > 0;
        }
       
    }
}
