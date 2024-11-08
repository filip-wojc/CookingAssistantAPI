using CookingAssistantAPI.DTO.Resources;
using CookingAssistantAPI.Services.IngredientServices;
using Microsoft.AspNetCore.Mvc;

namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/resources")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _service;
        public ResourceController(IResourceService service)
        {
            _service = service;
        }

        [HttpGet("ingredients")]
        [ResponseCache(Duration = 1200)]
        public async Task<ActionResult<List<string>>> GetAllIngredientNames()
        {
            var ingredients = await _service.GetAllIngredientsListAsync();
            return Ok(ingredients);
        }

        [HttpGet("difficulties")]
        [ResponseCache(Duration = 1200)]
        public async Task<ActionResult<List<DifficultiesGetDTO>>> GetAllDifficulties()
        {
            var difficulties = await _service.GetAllDifficultiesAsync();
            return Ok(difficulties);
        }

        [HttpGet("occasions")]
        [ResponseCache(Duration = 1200)]
        public async Task<ActionResult<List<OccasionsGetDTO>>> GetAllOccasions()
        {
            var occasions = await _service.GetAllOccasionsAsync();
            return Ok(occasions);
        }

        [HttpGet("categories")]
        [ResponseCache(Duration = 1200)]
        public async Task<ActionResult<List<CategoriesGetDTO>>> GetAllCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
