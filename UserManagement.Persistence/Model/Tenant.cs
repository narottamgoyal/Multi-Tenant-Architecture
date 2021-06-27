using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace UserManagement.Persistence
{
    public class Tenant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SupportContact { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string DomainName { get; set; }
        public bool IsActive { get; set; } = true;

        public List<string> Roles = new List<string>();
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime LastModifiedTimeStamp { get; set; }
    }
}