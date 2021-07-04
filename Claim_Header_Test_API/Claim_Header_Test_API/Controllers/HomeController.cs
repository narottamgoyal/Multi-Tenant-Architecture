using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api.Gateway.Controllers
{
    [EnableCors("corspolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class HomeController : ControllerBase
    {
        [HttpGet("info")]
        public IActionResult Get()
        {
            var Claims = from c in User.Claims select new { c.Type, c.Value };
            var Headers = from c in HttpContext.Request.Headers select new { c.Key, c.Value };
            return new JsonResult(new { Claims, Headers });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Info()
        {
            return StatusCode(StatusCodes.Status200OK, "Its running...!!");
        }
    }
}
