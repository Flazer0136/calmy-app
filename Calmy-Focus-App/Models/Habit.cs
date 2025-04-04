using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Calmy_Focus_App.Models
{
    public class Habit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("streak")]
        public int Streak { get; set; }

        [BsonElement("lastCompleted")]
        public DateTime? LastCompleted { get; set; }

        [BsonElement("history")]
        public List<DateTime> History { get; set; } = new List<DateTime>();
    }
}