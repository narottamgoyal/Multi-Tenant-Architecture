using System.Threading.Tasks;
using UserManagement.Persistence;

namespace TenantService
{
    public interface ITenantDetailService
    {
        Task AddTenantUserAsync(string domainName, UserDetail user);
        Task<UserDetail> GetTenantUserAsync(string domainName, string emailId);
        Task AddTenantAsync(Tenant tenant);
    }

    public class TenantDetailService : ITenantDetailService
    {
        private readonly ICacheService _cacheService;
        private readonly ITenantDbUserRepository _tenantDbUserRepository;
        private readonly IMasterDbTenantRepository _masterDbTenantRepository;
        public TenantDetailService(ICacheService cacheService, ITenantDbUserRepository tenantDbUserRepository, IMasterDbTenantRepository masterDbTenantRepository)
        {
            _cacheService = cacheService;
            _tenantDbUserRepository = tenantDbUserRepository;
            _masterDbTenantRepository = masterDbTenantRepository;
        }

        public async Task AddTenantUserAsync(string domainName, UserDetail user)
        {
            var tenant = await GetTenantAsync(domainName);
            if (tenant == null) return;
            await _tenantDbUserRepository.AddUserAsync(tenant.ConnectionString, tenant.DatabaseName, user);
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

        public async Task<UserDetail> GetTenantUserAsync(string domainName, string emailId)
        {
            var tenant = await GetTenantAsync(domainName);
            if (tenant == null) return null;
            return await _tenantDbUserRepository.GetUserAsync(tenant.ConnectionString, tenant.DatabaseName, emailId);
        }

        public async Task AddTenantAsync(Tenant tenant)
        {
            await _masterDbTenantRepository.AddTenantAsync(tenant);
            await _cacheService.SetCacheValueAsync(tenant.DomainName, tenant);
        }
    }
}
