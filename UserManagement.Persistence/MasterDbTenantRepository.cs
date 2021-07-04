using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDbClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Persistence
{
    public interface IMasterDbTenantRepository
    {
        Task<Tenant> AddTenantAsync(Tenant tenant);
        Task<Tenant> GetTenantAsync(string domainName);
    }

    public class MasterDbTenantRepository : MongoDbRepository<Tenant>, IMasterDbTenantRepository
    {
        public MasterDbTenantRepository(IDatabaseContext databaseContext) : base(databaseContext) { }

        public async Task<Tenant> AddTenantAsync(Tenant tenant)
        {
            tenant.CreatedTimeStamp = tenant.CreatedTimeStamp = DateTime.UtcNow;
            await collection.InsertOneAsync(tenant);
            return tenant;
        }

        public async Task<Tenant> GetTenantAsync(string domainName)
        {
            return await collection.AsQueryable().FirstOrDefaultAsync(x => x.DomainName == domainName && x.IsActive);
        }
    }
}
