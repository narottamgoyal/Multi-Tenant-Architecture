using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using UserManagement.Model;

namespace MongoDbClient
{
    public interface IMongoDbQueryRepository
    {
        IQueryable<T> Query<T>(string collectionName) where T : class, new();
        IMongoQueryable<T> Query<T>() where T : class, new();
    }

    public class MongoDbQueryRepository : IMongoDbQueryRepository
    {
        public IMongoDatabase _db;

        public MongoDbQueryRepository(IOptions<ConfigurationSetting> configuration)
        {
            var MongoClient = new MongoClient(configuration.Value.DatabaseSettings.ConnectionString);
            _db = MongoClient.GetDatabase(configuration.Value.DatabaseSettings.DatabaseName);
        }


        public IQueryable<T> Query<T>(string collectionName) where T : class, new()
        {
            return _db.GetCollection<T>(collectionName).AsQueryable();
        }

        public IMongoQueryable<T> Query<T>() where T : class, new()
        {
            return _db.GetCollection<T>(typeof(T).Name).AsQueryable();
        }
    }
}
