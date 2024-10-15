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
        public async Task<bool> AddUserToDbAsync(User user)
        {
            await _context.Users.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
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
