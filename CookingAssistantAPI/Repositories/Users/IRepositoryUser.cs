using CookingAssistantAPI.Database.Models;

namespace CookingAssistantAPI.Repositories.Users
{
    public interface IRepositoryUser
    {
        Task<bool> AddUserToDbAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        // Task<bool> RemoveUserFromDbAsync(int userId); FIX BEFORE USING
    }
}
