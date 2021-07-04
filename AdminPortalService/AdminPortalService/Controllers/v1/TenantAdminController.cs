using AdminPortalService.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TenantService;
using UserManagement.Persistence;

namespace AdminPortalService.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TenantAdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserDetailService _userDetailService;

        public TenantAdminController(IMapper mapper, IUserDetailService userDetailService)
        {
            _mapper = mapper;
            _userDetailService = userDetailService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(StatusCodes.Status200OK, "TenantAdmin v1 is running...");
        }

        /// <summary>
        /// Create Tenant user
        /// </summary>
        /// <param name="userDetailDto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("{role}")]
        public async Task<IActionResult> PostAsync([FromBody] UserDetailDto userDetailDto, TenantUserRole role)
        {
            var user = _mapper.Map<UserDetail>(userDetailDto);
            if (HttpContext.Request.Headers.ContainsKey("DomainName"))
            {
                user.EmailId += "@" + HttpContext.Request.Headers["DomainName"].ToString();
                if (role == TenantUserRole.AdminAndBasic)
                {
                    user.Roles.Add(TenantUserRole.TenantAdmin.ToString());
                    user.Roles.Add(TenantUserRole.BasicUser.ToString());
                }
                else user.Roles.Add(role.ToString());
                await _userDetailService.AddUserAsync(user);
            }
            return StatusCode(StatusCodes.Status200OK, "done");
        }
    }
}
