using Calmy_Focus_App.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Calmy_Focus_App.Services
{
    public class NotesService : INotesService
    {
        private readonly IMongoCollection<Note> _notesCollection;

        public NotesService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _notesCollection = database.GetCollection<Note>(mongoDBSettings.Value.NotesCollectionName);
        }

        public async Task<List<Note>> GetAsync() =>
            await _notesCollection.Find(_ => true).ToListAsync();

        public async Task<Note?> GetAsync(string id) =>
            await _notesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Note note) =>
            await _notesCollection.InsertOneAsync(note);

        public async Task UpdateAsync(string id, Note note)
        {
            var filter = Builders<Note>.Filter.Eq(n => n.Id, id);
            var update = Builders<Note>.Update
                .Set(n => n.Title, note.Title)
                .Set(n => n.Content, note.Content)
                .Set(n => n.UpdatedAt, DateTime.UtcNow); 

            await _notesCollection.UpdateOneAsync(filter, update);
        }


        public async Task RemoveAsync(string id) =>
            await _notesCollection.DeleteOneAsync(x => x.Id == id);
    }
}