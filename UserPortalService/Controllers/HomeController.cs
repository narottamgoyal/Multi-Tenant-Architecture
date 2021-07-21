using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagement.Persistence;
using UserManagement.Persistence.Model;

namespace UserPortalService.Controllers
{
    [Authorize(Roles = "BasicUser")]
    [EnableCors("corspolicy")]
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IUserMessageRepository _repository;

        public HomeController(IUserMessageRepository repo)
        {
            _repository = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (HttpContext.Request.Headers.ContainsKey("Email"))
            {
                var result = await _repository.Get(HttpContext.Request.Headers["Email"].ToString());
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string message)
        {
            if (HttpContext.Request.Headers.ContainsKey("Email"))
            {
                var result = await _repository.Add(new UserMessage { EmailId = HttpContext.Request.Headers["Email"].ToString(), Message = message });
                return StatusCode(StatusCodes.Status200OK, result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
