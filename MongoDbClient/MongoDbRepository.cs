using MongoDB.Driver;
using UserManagement.Model;

namespace MongoDbClient
{
    public abstract class MongoDbRepository<T>
    {
        protected IMongoDatabase _db;
        protected readonly IMongoCollection<T> collection;

        public MongoDbRepository(ConfigurationSetting configuration)
        {
            var MongoClient = new MongoClient(configuration.DatabaseSettings.ConnectionString);
            _db = MongoClient.GetDatabase(configuration.DatabaseSettings.DatabaseName);
            collection = _db.GetCollection<T>(typeof(T).Name);
        }

        public MongoDbRepository(IDatabaseContext databaseContext)
        {
            _db = databaseContext.Database;
            collection = _db.GetCollection<T>(typeof(T).Name);
        }
    }
}