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
        private readonly ITenantDetailService _tenantService;
        private readonly IMasterDbTenantRepository _masterDbTenantRepository;
        public UserDetailService(ICacheService cacheService, IMasterDbUserRepository masterDbUserRepository, ITenantDetailService tenantService, IMasterDbTenantRepository masterDbTenantRepository)
        {
            _cacheService = cacheService;
            _masterDbUserRepository = masterDbUserRepository;
            _tenantService = tenantService;
        }

        public async Task AddUserAsync(UserDetail user)
        {
            if (user == null) return;
            var domainName = user.EmailId.Split('@')[1];
            if (domainName == DefaultDomainName) await _masterDbUserRepository.AddUserAsync(user);
            await _tenantService.AddTenantUserAsync(domainName, user);
        }

        public async Task<UserDetail> GetUserAsync(string emailId)
        {
            if (string.IsNullOrWhiteSpace(emailId) && !emailId.Contains('@')) return null;
            var domainName = emailId.Split('@')[1];
            if (domainName == DefaultDomainName) return await _masterDbUserRepository.GetUserAsync(emailId);
            return await _tenantService.GetTenantUserAsync(domainName, emailId);
        }
    }
}
