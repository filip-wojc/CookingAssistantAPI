using Microsoft.AspNetCore.Mvc;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Repositories;
using CookingAssistantAPI.Services.UserServices;


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

        /*
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LogIn(UserRegisterDTO user)
        {

        }
        */

        // public async DeleteUser
        // public async ChangeRole


    }
}
