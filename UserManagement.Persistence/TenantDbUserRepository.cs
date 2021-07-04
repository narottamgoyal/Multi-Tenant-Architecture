using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Persistence
{
    public interface ITenantDbUserRepository
    {
        Task<UserDetail> AddUserAsync(string connectionString, string dbName, UserDetail user);
        Task<UserDetail> GetUserAsync(string connectionString, string dbName, string emailId);
    }

    public class TenantDbUserRepository : ITenantDbUserRepository
    {
        private IMongoCollection<UserDetail> GetCollection(string connectionString, string dbName)
        {
            return new MongoClient(connectionString).GetDatabase(dbName).GetCollection<UserDetail>(nameof(UserDetail));
        }

        public async Task<UserDetail> AddUserAsync(string connectionString, string dbName, UserDetail user)
        {
            user.CreatedTimeStamp = user.CreatedTimeStamp = DateTime.UtcNow;
            await GetCollection(connectionString, dbName).InsertOneAsync(user);
            return user;
        }

        public async Task<UserDetail> GetUserAsync(string connectionString, string dbName, string emailId)
        {
            return await GetCollection(connectionString, dbName).AsQueryable().FirstOrDefaultAsync(x => x.EmailId == emailId.ToLower() && x.IsActive);
        }
    }
}
