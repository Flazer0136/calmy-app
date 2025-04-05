namespace Calmy_Focus_App.Models
{
    public class MongoDBSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string NotesCollectionName { get; set; }
        public required string HabitsCollectionName { get; set; }
        public required string CalendarCollectionName { get; set; }
    }
}
