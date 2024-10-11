using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Repositories;
using CookingAssistantAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;


namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryRecipe _repository;
        public RecipeController(IRepositoryRecipe repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpPost]
        //[Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateRecipe([FromForm] RecipeCreateDTO recipeDto)
        {
            var recipe = _mapper.Map<Recipe>(recipeDto);
            bool isSuccess = await _repository.AddRecipeAsync(recipe);
            if (isSuccess)
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpGet("{*recipeName}")]
        public async Task<ActionResult<Recipe>> GetRecipeByName([FromRoute] string recipeName)
        {
            var recipe = await _repository.GetRecipeByNameAsync(recipeName);
            return Ok(recipe);
        }

    }
}
