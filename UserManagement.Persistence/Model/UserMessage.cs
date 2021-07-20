using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserManagement.Persistence.Model
{
    public class UserMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        private string _emailId;
        public string EmailId { get { return _emailId; } set { _emailId = value?.ToLower(); } }
        public string Message { get; set; }
    }
}
