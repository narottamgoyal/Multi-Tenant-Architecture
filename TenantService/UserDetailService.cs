using System.Threading.Tasks;
using UserManagement.Persistence;

namespace TenantService
{

    public interface IUserDetailService
    {
        Task<UserDetail> GetUserAsync(string username);
        Task AddUserAsync(UserDetail user);
    }

    public class UserDetailService : IUserDetailService
    {
        private const string DefaultDomainName = "infinitepoc.com";
        private readonly ICacheService _cacheService;
        private readonly IMasterDbUserRepository _masterDbUserRepository;
        private readonly ITenantDbUserRepository _tenantDbUserRepository;
        private readonly IMasterDbTenantRepository _masterDbTenantRepository;
        public UserDetailService(ICacheService cacheService, IMasterDbUserRepository masterDbUserRepository, ITenantDbUserRepository tenantDbUserRepository, IMasterDbTenantRepository masterDbTenantRepository)
        {
            _cacheService = cacheService;
            _masterDbUserRepository = masterDbUserRepository;
            _tenantDbUserRepository = tenantDbUserRepository;
            _masterDbTenantRepository = masterDbTenantRepository;
        }

        public async Task AddUserAsync(UserDetail user)
        {
            if (user == null) return;
            var domainName = user.EmailId.Split('@')[1];
            if (domainName == DefaultDomainName) await _masterDbUserRepository.AddUserAsync(user);
            await AddTenantUserAsync(domainName, user);
        }

        private async Task AddTenantUserAsync(string domainName, UserDetail user)
        {
            var tenant = await GetTenantAsync(domainName);
            if (tenant == null) return;
            await _tenantDbUserRepository.AddUserAsync(tenant.ConnectionString, tenant.DatabaseName, user);
        }

        public async Task<UserDetail> GetUserAsync(string emailId)
        {
            if (string.IsNullOrWhiteSpace(emailId) && !emailId.Contains('@')) return null;
            var domainName = emailId.Split('@')[1];
            if (domainName == DefaultDomainName) return await _masterDbUserRepository.GetUserAsync(emailId);
            return await GetTenantUserAsync(domainName, emailId);
        }

        private async Task<UserDetail> GetTenantUserAsync(string domainName, string emailId)
        {
            var tenant = await GetTenantAsync(domainName);
            if (tenant == null) return null;
            return await _tenantDbUserRepository.GetUserAsync(tenant.ConnectionString, tenant.DatabaseName, emailId);
        }

        private async Task<Tenant> GetTenantAsync(string domainName)
        {
            // Search in cache
            var tenantObj = await _cacheService.GetCacheValueAsync(domainName);
            if (tenantObj != null) return Newtonsoft.Json.JsonConvert.DeserializeObject<Tenant>(tenantObj);

            // Search in master db
            var tenant = await _masterDbTenantRepository.GetTenantAsync(domainName);
            if (tenant != null) await _cacheService.SetCacheValueAsync(domainName, tenant);
            return tenant;
        }
    }
}
