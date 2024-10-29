using CookingAssistantAPI.Services.NutrientServices;
using Microsoft.AspNetCore.Mvc;

namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/nutrients")]
    public class NutrientsController : ControllerBase
    {
        private readonly INutrientService _service;
        public NutrientsController(INutrientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllNutrientNames()
        {
            var nutrients = await _service.GetAllNutrientsListAsync();
            return Ok(nutrients);
        }
    }
}
