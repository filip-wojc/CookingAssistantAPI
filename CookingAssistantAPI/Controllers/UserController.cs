using Microsoft.AspNetCore.Mvc;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Repositories;
using CookingAssistantAPI.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.Tools;
using CookingAssistantAPI.DTO;
using CookingAssistantAPI.Services.RecipeServices;


namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IRecipePaginationService _paginationService;
        public UserController(IUserService service, IRecipePaginationService paginationService)
        {
            _service = service;
            _paginationService = paginationService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDTO user)
        {
            if (await _service.RegisterUser(user))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult> LoginUser([FromBody] UserLoginDTO user)
        {
            string token = await _service.GenerateToken(user);
            var response = new LogInResponseDTO { Token = token };
            return Ok(response);
        }

        [HttpPost("favourite/{recipeId}")]
        [Authorize]
        public async Task<ActionResult> AddRecipeToFavourites([FromRoute] int recipeId)
        {
            if (await _service.AddRecipeToFavourites(recipeId))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpGet("favourite-recipes")]
        [Authorize]
        public async Task<ActionResult<PageResult<RecipeSimpleGetDTO>>> GetFavouriteRecipes([FromQuery] RecipeQuery query)
        {
            var favouriteRecipes = await _service.GetFavouriteRecipesAsync(query);
            var pageResult = _paginationService.GetPaginatedResult(query, favouriteRecipes);
            return Ok(pageResult);
        }

        [HttpPost("image")]
        [Authorize]
        public async Task<ActionResult> AddProfilePicture([FromForm] UploadFileDTO profilePicture)
        {
            if (await _service.UploadProfilePicture(profilePicture))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpDelete("delete/{userName}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser([FromRoute] string userName)
        {
            if (await _service.DeleteUserAsync(userName))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("favourite-recipes/delete/{recipeId}")]
        [Authorize]
        public async Task<ActionResult> RemoveRecipeFromFavourites([FromRoute] int recipeId)
        {
            if (await _service.RemoveRecipeFromFavouritesAsync(recipeId))
            {
                return NoContent();
            }
            return BadRequest();
        }

        // public async ModifyProfile {ProfilePic, UserName}
    }
}
