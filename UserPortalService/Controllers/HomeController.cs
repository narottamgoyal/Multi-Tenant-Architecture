using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagement.Persistence;
using UserManagement.Persistence.Model;

namespace UserPortalService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IUserMessageRepository _repository;

        public HomeController(IUserMessageRepository repo)
        {
            _repository = repo;
        }

        [HttpGet("email")]
        public async Task<IActionResult> Get(string email)
        {
            var result = await _repository.Get(email);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost("email")]
        public async Task<IActionResult> Post([FromBody] string message, string email)
        {
            var result = await _repository.Add(new UserMessage { EmailId = email, Message = message });
            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
