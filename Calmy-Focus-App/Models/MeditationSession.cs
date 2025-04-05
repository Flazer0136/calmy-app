using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Calmy_Focus_App.Models
{
    public class MeditationSession
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string TrackName { get; set; }

        [Required]
        [Range(1, 120)]
        public int DurationMinutes { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? EndTime { get; set; } // Add this property

        public bool Completed { get; set; } = false;
    }
}