// Models/CalendarEvent.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Calmy_Focus_App.Models
{
    public class Calendar
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault] 
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; } = false;
        public string Color { get; set; } = "#3a87ad";
    }
}