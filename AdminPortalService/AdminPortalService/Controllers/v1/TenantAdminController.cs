using AdminPortalService.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TenantService;
using UserManagement.Persistence;

namespace AdminPortalService.Controllers.v1
{
    [Authorize(Roles = "TenantAdmin2,TenantAdmin")]
    [EnableCors("corspolicy")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TenantAdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITenantDetailService _tenantService;

        public TenantAdminController(IMapper mapper, ITenantDetailService tenantService)
        {
            _mapper = mapper;
            _tenantService = tenantService;
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
            var domainName = HttpContext.Request.Headers.ContainsKey("DomainName") ?
                HttpContext.Request.Headers["DomainName"].ToString() : null;
            if (!string.IsNullOrWhiteSpace(domainName))
            {
                user.EmailId += "@" + domainName;
                if (role == TenantUserRole.AdminAndBasic)
                {
                    user.Roles.Add(TenantUserRole.TenantAdmin.ToString());
                    user.Roles.Add(TenantUserRole.BasicUser.ToString());
                }
                else user.Roles.Add(role.ToString());
                await _tenantService.AddTenantUserAsync(domainName, user);
            }
            return StatusCode(StatusCodes.Status200OK, "done");
        }
    }
}
