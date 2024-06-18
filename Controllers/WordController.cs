using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TechDictionaryApi.DTOs;
using TechDictionaryApi.Repositories;
using TechDictionaryApi.Services;

namespace TechDictionaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IWordService _wordService;
        public WordController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [Authorize]
        [HttpPost("CreateWordAndExamples")]
        public async Task<IActionResult> CreateWordAndExamples([FromBody] CreateWordAndExamplesDTO request)
        {
            string UserId = this.User.Claims.ToList()[0].Value;
            string Username = this.User.Claims.ToList()[1].Value;
            string Email = this.User.Claims.ToList()[2].Value;
            string RoleId = this.User.Claims.ToList()[3].Value;

            string createdBy = Username; //use username for updatedBy

            var response = await _wordService.CreateWordAndExamples(request, createdBy);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("UpdateWordAndExamples")]
        public async Task<IActionResult> UpdateWordAndExamples([FromBody] UpdateWordAndExamplesDTO request)
        {
            string UserId = this.User.Claims.ToList()[0].Value;
            string Username = this.User.Claims.ToList()[1].Value;
            string Email = this.User.Claims.ToList()[2].Value;
            string RoleId = this.User.Claims.ToList()[3].Value;

            string updatedBy = Username; //use username for updatedBy

            var response = await _wordService.UpdateWordAndExamples(request, updatedBy);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("DeleteWordAndExamples")]
        public async Task<IActionResult> DeleteWordAndExamples([FromBody] DeleteWordAndExamplesDTO request)
        {
            string UserId = this.User.Claims.ToList()[0].Value;
            string Username = this.User.Claims.ToList()[1].Value;
            string Email = this.User.Claims.ToList()[2].Value;
            string RoleId = this.User.Claims.ToList()[3].Value;

            string deletedBy = Username; //use username for deletedBy

            var response = await _wordService.DeleteWordAndExamples(request, deletedBy);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetAllWordsAndExamples")]
        public async Task<IActionResult> GetAllWordsAndExamples()
        {
            var response = await _wordService.GetAllWordsAndExamples();
            return Ok(response);
        }       

        [Authorize]
        [HttpGet("GetAdminDashboard")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var response = await _wordService.GetAdminDashboard();
            return Ok(response);
        }

        [HttpGet("SearchForWord")]
        public async Task<IActionResult> GetWordByWord([FromQuery] string word)
        {
            var response = await _wordService.GetWordByWord(word);
            return Ok(response);
        }
    }
}
