using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace UserManagement.Persistence
{
    public class UserDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        private string _emailId;
        public string EmailId { get { return _emailId; } set { _emailId = value?.ToLower(); } }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsActive { get; set; } = true;

        public List<string> Roles = new List<string>();
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime LastModifiedTimeStamp { get; set; }
    }
}