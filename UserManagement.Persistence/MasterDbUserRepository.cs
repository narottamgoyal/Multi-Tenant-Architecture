using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDbClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Persistence
{
    public interface IMasterDbUserRepository
    {
        Task<UserDetail> AddUserAsync(UserDetail user);
        Task<UserDetail> GetUserAsync(string emailId);
    }

    public class MasterDbUserRepository : MongoDbRepository<UserDetail>, IMasterDbUserRepository
    {
        public MasterDbUserRepository(IDatabaseContext databaseContext) : base(databaseContext) { }
        public async Task<UserDetail> AddUserAsync(UserDetail user)
        {
            user.CreatedTimeStamp = user.CreatedTimeStamp = DateTime.UtcNow;
            await collection.InsertOneAsync(user);
            return user;
        }

        public async Task<UserDetail> GetUserAsync(string emailId)
        {
            return await collection.AsQueryable().FirstOrDefaultAsync(x => x.EmailId == emailId.ToLower() && x.IsActive);
        }
    }
}
