using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using CookingAssistantAPI.DTO.Recipes;
using Microsoft.AspNetCore.Authorization;
using CookingAssistantAPI.DTO.Reviews;
using CookingAssistantAPI.Services.ReviewServices;
using CookingAssistantAPI.Tools;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CookingAssistantAPI.Services.RecipeServices;
using Swashbuckle.AspNetCore.Annotations;


namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _service;
        public RecipeController(IRecipeService service, IReviewService reviewService)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecipe([FromForm] RecipeCreateDTO recipeDto)
        {
            if (await _service.AddRecipe(recipeDto))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpPut("{*recipeId}")]
        public async Task<ActionResult> ModifyRecipe([FromRoute] int recipeId, [FromForm] RecipeCreateDTO recipeDto)
        {
            if (await _service.ModifyRecipeAsync(recipeDto, recipeId))
            {
                return Ok();
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
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "SearchPhrase", "IngredientsSearch", "FilterByDifficulty", "FilterByCategoryName", "FilterByOccasion", "PageNumber", "PageSize", "SortBy", "SortDirection" })]
        public async Task<ActionResult<PageResult<RecipeSimpleGetDTO>>> GetAllRecipes([FromQuery] RecipeQuery query)
        {
            var recipes = await _service.GetAllRecipesAsync(query);
            return Ok(recipes);
        }

        [HttpGet("names")]
        [AllowAnonymous]
        [ResponseCache(Duration = 1200)]
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

        [HttpGet("pdf/{recipeId}")]
        public async Task<ActionResult> GeneratePdf([FromRoute] int recipeId)
        {
            var pdfStream = await _service.GetRecipePdf(recipeId);
            return File(pdfStream.ToArray(), "application/pdf",$"recipe_{recipeId}.pdf");
        }

    }
}
