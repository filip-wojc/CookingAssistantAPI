using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Repositories.Users;
using Microsoft.AspNetCore.Identity;

namespace CookingAssistantAPI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUser _repository;
        private readonly IPasswordHasher<User> _hasher;
        public UserService(IRepositoryUser repository, IPasswordHasher<User> hasher)
        {
            _hasher = hasher;
            _repository = repository;
        }
        public async Task<bool> RegisterUser(UserRegisterDTO dto)
        {
            var newUser = new User()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                RoleId = 1
            };

            newUser.PasswordHash = _hasher.HashPassword(newUser, dto.Password);
            if(await _repository.AddUserToDbAsync(newUser))
            {
                return true;
            }
            return false;
        }
    }
}
