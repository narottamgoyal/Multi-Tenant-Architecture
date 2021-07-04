using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortalService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupperAdminController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(StatusCodes.Status200OK, "Its running...");
        }
    }
}
