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
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDTO user)
        {
            if (await _service.RegisterUser(user))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] UserLoginDTO user)
        {
            string token = await _service.GenerateToken(user);
            var response = new LogInResponseDTO { Token = token };
            return Ok(response);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] UserPasswordChangeDTO dto)
        {
            if (await _service.ChangeUserPassword(dto))
            {
                return Ok();
            }
            return BadRequest();
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
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "SearchPhrase", "IngredientsSearch", "FilterByDifficulty", "FilterByCategoryName", "FilterByOccasion", "PageNumber", "PageSize", "SortBy", "SortDirection" })]
        [Authorize]
        public async Task<ActionResult<PageResult<RecipeSimpleGetDTO>>> GetFavouriteRecipes([FromQuery] RecipeQuery query)
        {
            var favouriteRecipes = await _service.GetFavouriteRecipesAsync(query);
            return Ok(favouriteRecipes);
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

        [HttpGet("image")]
        [Authorize]
        public async Task<ActionResult<byte[]>> GetUserProfilePicture()
        {
            var imageData = await _service.GetUserProfilePictureAsync();
            return File(imageData, "image/jpeg");
        }

        [HttpDelete("delete/{password}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser([FromRoute] string password)
        {
            if (await _service.DeleteUserAsync(password))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("favourite-recipes/{recipeId}")]
        [Authorize]
        public async Task<ActionResult> RemoveRecipeFromFavourites([FromRoute] int recipeId)
        {
            if (await _service.RemoveRecipeFromFavouritesAsync(recipeId))
            {
                return NoContent();
            }
            return BadRequest();
        }

       
    }
}
