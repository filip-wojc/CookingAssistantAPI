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
        private readonly IRecipeQueryService _recipeQueryService;
        private readonly IMapper _mapper;
        public UserService(IRepositoryUser repository, IPasswordHasher<User> hasher,
            JwtParameters jwtParameters, IUserContextService userContext, IRecipeQueryService recipeQueryService, IMapper mapper)
        {
            _hasher = hasher;
            _repository = repository;
            _jwtParameters = jwtParameters;
            _userContext = userContext;
            _recipeQueryService = recipeQueryService;
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

        public async Task<bool> AddRecipeToFavourites(int recipeId)
        {
            if (await _repository.AddRecipeToFavourites(recipeId, _userContext.UserId))
            {
                return true;
            }
            return false;
        }

        public async Task<List<RecipeSimpleGetDTO>> GetFavouriteRecipesAsync(RecipeQuery query)
        {
            var recipes = await _repository.GetFavouriteRecipesAsync(_userContext.UserId);
            var recipeDtos = _mapper.Map<List<RecipeSimpleGetDTO>>(recipes);

            recipeDtos = _recipeQueryService.SearchRecipes(ref recipeDtos, query.SearchPhrase);
            recipeDtos = _recipeQueryService.SortRecipes(ref recipeDtos, query.SortBy, query.SortDirection);
            recipeDtos = _recipeQueryService.RecipeFilter(ref recipeDtos, query.FilterByCategoryName, query.FilterByDifficulty);

            return recipeDtos;
        }

        public async Task<bool> UploadProfilePicture(UploadFileDTO profilePicture)
        {
            var profilePictureByteArray = _mapper.Map<byte[]>(profilePicture.formFile);
            if (await _repository.UploadProfilePicture(_userContext.UserId, profilePictureByteArray))
            {
                return true;
            }
            return false;
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
