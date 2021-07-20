using MongoDB.Driver;

namespace MongoDbClient
{
    public interface IDatabaseContext
    {
        IMongoDatabase Database { get; }
        MongoClient MongoClient { get; }
    }

    public class DatabaseContext : IDatabaseContext
    {
        public IMongoDatabase Database { get; }
        public MongoClient MongoClient { get; }
        public DatabaseContext(string connectionString, string databaseName)
        {
            MongoClient = new MongoClient(connectionString);
            Database = MongoClient.GetDatabase(databaseName);
        }
    }
}
