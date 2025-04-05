// Services/CalendarService.cs
using Calmy_Focus_App.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Calmy_Focus_App.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IMongoCollection<CalendarEvent> _calendarEvents;

        public CalendarService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _calendarEvents = database.GetCollection<CalendarEvent>(mongoDBSettings.Value.CalendarCollectionName);
        }
        
        public async Task<List<CalendarEvent>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var filter = Builders<CalendarEvent>.Filter.And(
                Builders<CalendarEvent>.Filter.Gte(x => x.Start, start),
                Builders<CalendarEvent>.Filter.Lte(x => x.End, end)
            );
    
            return await _calendarEvents.Find(filter).ToListAsync();
        }

        public async Task<List<CalendarEvent>> GetAsync() =>
            await _calendarEvents.Find(_ => true).ToListAsync();

        public async Task<CalendarEvent?> GetAsync(string id) =>
            await _calendarEvents.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(CalendarEvent calendarEvent) =>
            await _calendarEvents.InsertOneAsync(calendarEvent);

        public async Task UpdateAsync(string id, CalendarEvent calendarEvent) =>
            await _calendarEvents.ReplaceOneAsync(x => x.Id == id, calendarEvent);

        public async Task RemoveAsync(string id) =>
            await _calendarEvents.DeleteOneAsync(x => x.Id == id);
        
    }
}