using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Exceptions;
using CookingAssistantAPI.Repositories.Users;
using CookingAssistantAPI.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CookingAssistantAPI.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUser _repository;
        private readonly IPasswordHasher<User> _hasher;
        private readonly JwtParameters _jwtParameters;
        public UserService(IRepositoryUser repository, IPasswordHasher<User> hasher, JwtParameters jwtParameters)
        {
            _hasher = hasher;
            _repository = repository;
            _jwtParameters = jwtParameters;
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

        public async Task<string> GenerateToken(UserLoginDTO dto)
        {
            var user = await _repository.GetUserByEmailAsync(dto.Email);

            var passwordVerification = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtParameters.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_jwtParameters.JwtExpireDays);

            var token = new JwtSecurityToken(_jwtParameters.JwtIssuer, _jwtParameters.JwtIssuer, claims,
                expires: expires, signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

        }

        // ADD USER ACCOUNT DELETE REQUEST VALIDATION HERE
        /*
        public async Task<bool> DeleteUser(int userId)
        {
            if (await _repository.RemoveUserFromDbAsync(userId))
            {
                return true;
            }
            return false;
        }
        */
    }
}
