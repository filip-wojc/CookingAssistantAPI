using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Route("api/sample")]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetHello()
        {
            return Ok("Hello world");
        }
    }
}
