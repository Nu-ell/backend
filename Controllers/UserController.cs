using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechDictionaryApi.DTOs;
using TechDictionaryApi.Repositories;
using TechDictionaryApi.Services;

namespace TechDictionaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var response = await _userService.Login(request);
            return Ok(response);
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromQuery] string userName)
        {
            var response = await _userService.LogOut(userName);
            return Ok(response);
        }
    }
}
