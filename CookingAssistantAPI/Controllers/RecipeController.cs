using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using CookingAssistantAPI.Services;
using CookingAssistantAPI.DTO.Recipes;
using Microsoft.AspNetCore.Authorization;
using CookingAssistantAPI.DTO.Reviews;
using CookingAssistantAPI.Services.ReviewServices;
using CookingAssistantAPI.Tools;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _service;
        private readonly IReviewService _reviewService;
        private readonly IPaginationService _paginationService;
        public RecipeController(IRecipeService service, IReviewService reviewService, IPaginationService paginationService)
        {
            _service = service;
            _reviewService = reviewService;
            _paginationService = paginationService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateRecipe([FromForm] RecipeCreateDTO recipeDto)
        {
            if (await _service.AddRecipe(recipeDto))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpDelete("{*recipeId}")]
        public async Task<ActionResult> DeleteRecipe([FromRoute] int recipeId)
        {
            if (await _service.DeleteRecipeByIdAsync(recipeId))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpGet("{*recipeId}")]
        public async Task<ActionResult<RecipeGetDTO>> GetRecipeById([FromRoute] int recipeId)
        {
            var recipe = await _service.GetRecipeByIdAsync(recipeId);
            return Ok(recipe);
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<RecipeSimpleGetDTO>>> GetAllRecipes([FromQuery] RecipeQuery query)
        {
            var recipes = await _service.GetAllRecipesAsync(query);
            var pageResult = _paginationService.GetPaginatedResult(query, recipes);
            return Ok(pageResult);
        }

        [HttpGet("names")]
        public async Task<ActionResult<List<RecipeNamesGetDTO>>> GetAllRecipesNames()
        {
            var recipes = await _service.GetAllRecipesNamesAsync();
            return Ok(recipes);
        }

        [HttpGet("image/{recipeId}")]
        public async Task<ActionResult<byte[]>> GetRecipeImage([FromRoute] int recipeId)
        {
            var imageData = await _service.GetRecipeImageAsync(recipeId);
            return File(imageData, "image/jpeg");
        }

        [HttpGet("nutrientsList")]
        public async Task<ActionResult<List<string>>> GetNutrientsList()
        {
            var nutrients = await _service.GetAllNutrientsAsync();
            return Ok(nutrients);
        }
        [HttpGet("ingredientsList")]
        public async Task<ActionResult<List<string>>> GetIngredientsList()
        {
            var ingredients = await _service.GetAllIngredientsAsync();
            return Ok(ingredients);
        }

    }
}
