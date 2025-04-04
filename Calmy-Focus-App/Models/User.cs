using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Calmy_Focus_App.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Email { get; set; }
        
        public string PasswordHash { get; set; }
    }
}