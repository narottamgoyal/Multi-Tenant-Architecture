using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace UserManagement.Persistence
{
    public class Tenant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SupportContact { get; set; }
        public string ConnectionString { get; set; }

        private string _dbName;
        public string DatabaseName { get { return _dbName; } set { _dbName = $"Tenant_{DomainName}"; } }
        private string _domainName;
        public string DomainName { get { return _domainName; } set { _domainName = value?.ToLower(); } }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedTimeStamp { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedTimeStamp { get; set; } = DateTime.UtcNow;
    }
}