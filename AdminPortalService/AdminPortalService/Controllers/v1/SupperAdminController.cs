using AdminPortalService.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TenantService;
using UserManagement.Persistence;

namespace AdminPortalService.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SupperAdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserDetailService _userDetailService;
        private readonly ITenantDetailService _tenantService;


        public SupperAdminController(IMapper mapper, IUserDetailService userDetailService, ITenantDetailService tenantService)
        {
            _mapper = mapper;
            _userDetailService = userDetailService;
            _tenantService = tenantService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(StatusCodes.Status200OK, "SupperAdmin v1 is running...");
        }

        /// <summary>
        /// Create Super Admin user
        /// </summary>
        /// <param name="userDetailDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserDetailDto userDetailDto)
        {
            var user = _mapper.Map<UserDetail>(userDetailDto);
            if (HttpContext.Request.Headers.ContainsKey("DomainName"))
            {
                user.EmailId += "@" + HttpContext.Request.Headers["DomainName"].ToString();
                user.Roles.Add(AdminUserRole.SuperAdmin.ToString());
                await _userDetailService.AddUserAsync(user);
            }
            return StatusCode(StatusCodes.Status200OK, "done");
        }

        /// <summary>
        /// Create Tenant
        /// </summary>
        /// <param name="tenantDto"></param>
        /// <returns></returns>
        [HttpPost("tenant")]
        public async Task<IActionResult> CreateTenantAsync([FromBody] TenantDto tenantDto)
        {
            var tenant = _mapper.Map<Tenant>(tenantDto);
            tenant.DatabaseName = $"Tenant_{ Regex.Replace(tenantDto.DomainName, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled) }";
            await _tenantService.AddTenantAsync(tenant);
            return StatusCode(StatusCodes.Status200OK, "done");
        }

        /// <summary>
        /// Create Tenant user
        /// </summary>
        /// <param name="userDetailDto"></param>
        /// <param name="role"></param>
        /// <param name="domainName"></param>
        /// <returns></returns>
        [HttpPost("tenant/user/{role}/{domainName}")]
        public async Task<IActionResult> CreateTenantUserAsync([FromBody] UserDetailDto userDetailDto, TenantUserRole role, string domainName)
        {
            var user = _mapper.Map<UserDetail>(userDetailDto);
            user.EmailId += "@" + domainName;
            if (role == TenantUserRole.AdminAndBasic)
            {
                user.Roles.Add(TenantUserRole.TenantAdmin.ToString());
                user.Roles.Add(TenantUserRole.BasicUser.ToString());
            }
            else user.Roles.Add(role.ToString());
            await _tenantService.AddTenantUserAsync(domainName, user);
            return StatusCode(StatusCodes.Status200OK, "done");
        }
    }
}
