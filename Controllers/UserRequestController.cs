using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechDictionaryApi.DTOs;
using TechDictionaryApi.Services;

namespace TechDictionaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRequestController : ControllerBase
    {
        private readonly IUserRequestService _userRequestService;

        public UserRequestController(IUserRequestService userRequestService)
        {
            _userRequestService = userRequestService;
        }

        //since user request can be made by both GeneralUser and Admin, the api should not have authorization with token so that everyone can access it.
        //This is why there is no "[Authorize]" here
        [HttpPost("UserRequest")]
        public async Task<IActionResult> RequestChangeToWordorRequestNewWord([FromBody] UserRequestDTO request)
        {
            var response = await _userRequestService.RequestChangeToWordorRequestNewWord(request);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("ResolveRequest")]
        public async Task<IActionResult> ResolveRequest([FromQuery] long userRequestId)
        {
            //Below code will read values for each claims encrypted in the token
            string UserId = this.User.Claims.ToList()[0].Value;
            string Username = this.User.Claims.ToList()[1].Value;
            string Email = this.User.Claims.ToList()[2].Value;
            string RoleId = this.User.Claims.ToList()[3].Value;

            string resolvedBy = Username; //use username for resolved by

            var response = await _userRequestService.ResolveRequest(userRequestId, resolvedBy);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetAllUserRequests")]
        public async Task<IActionResult> GetAllUserRequests()
        {
            //since we are not going to insert cretedby or updatedby ...etc. we will not be reading from token for get methods

            //string UserId = this.User.Claims.ToList()[0].Value;
            //string Username = this.User.Claims.ToList()[1].Value;
            //string Email = this.User.Claims.ToList()[2].Value;
            //string RoleId = this.User.Claims.ToList()[3].Value;

            var response = await _userRequestService.GetAllUserRequests();
            return Ok(response);
        }

        [HttpGet("GetGeneralUserDashboard")]
        public async Task<IActionResult> GetGeneralUserDashboard()
        {
            var response = await _userRequestService.GetGeneralUserDashboard();
            return Ok(response);
        }
    }
}
