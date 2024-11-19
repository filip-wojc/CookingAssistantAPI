using AutoMapper;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Exceptions;
using CookingAssistantAPI.Repositories.Users;
using CookingAssistantAPI.Services.RecipeServices;
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
        private readonly IUserContextService _userContext;
        private readonly IPasswordHasher<User> _hasher;
        private readonly JwtParameters _jwtParameters;
        private readonly IMapper _mapper;
        public UserService(IRepositoryUser repository, IPasswordHasher<User> hasher,
            JwtParameters jwtParameters, IUserContextService userContext, IMapper mapper)
        {
            _hasher = hasher;
            _repository = repository;
            _jwtParameters = jwtParameters;
            _userContext = userContext;
            _mapper = mapper;
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
            return await _repository.AddUserToDbAsync(newUser);
        }

        public async Task<LogInResponseDTO> GenerateToken(UserLoginDTO dto)
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
            return new LogInResponseDTO {Token = tokenHandler.WriteToken(token), UserName = user.UserName, Email = user.Email};

        }

        public async Task<bool> AddRecipeToFavourites(int recipeId)
        {
            return await _repository.AddRecipeToFavourites(recipeId, _userContext.UserId);
        }

        public async Task<PageResult<RecipeSimpleGetDTO>> GetFavouriteRecipesAsync(RecipeQuery query)
        {
            var recipes = await _repository.GetPaginatedFavouriteRecipesAsync(_userContext.UserId, query);
            var recipeDtos = _mapper.Map<List<RecipeSimpleGetDTO>>(recipes.Item1);
            return new PageResult<RecipeSimpleGetDTO>(recipeDtos, recipes.Item2, query.PageSize ?? 10, query.PageNumber ?? 1);
        }

        public async Task<PageResult<RecipeSimpleGetDTO>> GetUserRecipesAsync(RecipeQuery query)
        {
            var recipes = await _repository.GetPaginatedUserRecipesAsync(_userContext.UserId, query);
            var recipeDtos = _mapper.Map<List<RecipeSimpleGetDTO>>(recipes.Item1);
            return new PageResult<RecipeSimpleGetDTO>(recipeDtos, recipes.Item2, query.PageSize ?? 10, query.PageNumber ?? 1);
        }

        public async Task<bool> UploadProfilePicture(UploadFileDTO profilePicture)
        {
            var profilePictureByteArray = _mapper.Map<byte[]>(profilePicture.imageData);
            return await _repository.UploadProfilePicture(_userContext.UserId, profilePictureByteArray);
        }

        public async Task<bool> DeleteUserAsync(PasswordDTO password)
        {
            var user = await _repository.GetUserByEmailAsync(_userContext.Email);
            var passwordVerification = _hasher.VerifyHashedPassword(user, user.PasswordHash, password.Password);
            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid password");
            }

            return await _repository.RemoveUserFromDbAsync(_userContext.UserId);
        }

        public async Task<bool> RemoveRecipeFromFavouritesAsync(int recipeId)
        {
            return await _repository.RemoveRecipeFromFavouritesAsync(_userContext.UserId, recipeId);      
        }

        public async Task<byte[]> GetUserProfilePictureAsync()
        {
            var profilePicture = await _repository.GetProfilePictureAsync(_userContext.UserId);
            return profilePicture;
        }

        public async Task<bool> ChangeUserPassword(UserPasswordChangeDTO dto)
        {
            var user = await _repository.GetUserByEmailAsync(_userContext.Email);

            var passwordVerification = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid old password");
            }

            dto.NewPassword = _hasher.HashPassword(user, dto.NewPassword);
            return await _repository.ChangePasswordAsync(_userContext.UserId, dto);
        }

        public async Task<bool> IsRecipeInFavouritesAsync(int recipeId)
        {
            return await _repository.IsRecipeInFavouritesAsync(_userContext.UserId, recipeId);
        }

        public async Task<bool> IsRecipeCreatedByUserAsync(int recipeId)
        {
            return await _repository.IsRecipeCreatedByUserAsync(_userContext.UserId, recipeId);
        }
    }
}
