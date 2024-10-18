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


namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _service;
        private readonly IReviewService _reviewService;
        public RecipeController(IRecipeService service, IReviewService reviewService)
        {
            _service = service;
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize]
        //[Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateRecipe([FromForm] RecipeCreateDTO recipeDto)
        {
            if (await _service.AddRecipe(recipeDto))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpGet("{*recipeId}")]
        public async Task<ActionResult<RecipeGetDTO>> GetRecipeById([FromRoute] int recipeId)
        {
            var recipe = await _service.GetRecipeByIdAsync(recipeId);
            return Ok(recipe);
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

        [HttpPost("{recipeId}/review")]
        [Authorize]
        public async Task<ActionResult> CreateReview([FromRoute] int recipeId, ReviewCreateDTO dto)
        {
            if (await _reviewService.AddReviewAsync(dto, recipeId))
            {
                return Created();
            }
            return BadRequest();
        }
    }
}
