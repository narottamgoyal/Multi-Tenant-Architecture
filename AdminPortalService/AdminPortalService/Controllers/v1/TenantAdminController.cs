using AdminPortalService.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Persistence;

namespace AdminPortalService.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TenantAdminController : ControllerBase
    {
        private readonly IMapper _mapper;

        public TenantAdminController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(StatusCodes.Status200OK, "TenantAdmin v1 is running...");
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDetailDto userDetailDto)
        {
            var user = _mapper.Map<UserDetail>(userDetailDto);
            return StatusCode(StatusCodes.Status200OK, "TenantAdmin v1 is running...");
        }
    }
}
