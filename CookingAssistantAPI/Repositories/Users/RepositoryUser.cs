using CookingAssistantAPI.Database;
using CookingAssistantAPI.Database.Models;

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
    }
}
