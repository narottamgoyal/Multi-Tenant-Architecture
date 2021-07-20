using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDbClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Persistence.Model;

namespace UserManagement.Persistence
{
    public interface IUserMessageRepository
    {
        Task<UserMessage> Add(UserMessage user);
        Task<List<UserMessage>> Get(string email);
    }

    public class UserMessageRepository : MongoDbRepository<UserMessage>, IUserMessageRepository
    {
        public UserMessageRepository(IDatabaseContext databaseContext) : base(databaseContext) { }
        public async Task<UserMessage> Add(UserMessage user)
        {
            await collection.InsertOneAsync(user);
            return user;
        }

        public async Task<List<UserMessage>> Get(string email)
        {
            return await collection.AsQueryable().Where(x => x.EmailId == email).ToListAsync();
        }
    }
}
