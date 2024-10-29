using CookingAssistantAPI.Services.IngredientServices;
using Microsoft.AspNetCore.Mvc;

namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/ingredients")]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _service;
        public IngredientsController(IIngredientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllIngredientNames()
        {
            var ingredients = await _service.GetAllIngredientsListAsync();
            return Ok(ingredients);
        }
    }
}
