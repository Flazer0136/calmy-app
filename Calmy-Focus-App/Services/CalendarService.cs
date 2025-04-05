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
        private readonly IMongoCollection<Calendar> _calendars;
        private readonly ILogger<CalendarService> _logger;
        
        public CalendarService(IOptions<MongoDBSettings> mongoDBSettings, 
            ILogger<CalendarService> logger)
        {
            _logger = logger;
        
            try 
            {
                _logger.LogInformation("Initializing CalendarService...");
            
                var settings = mongoDBSettings.Value;
                _logger.LogInformation($"Connecting to: {settings.ConnectionString}");
                _logger.LogInformation($"Database: {settings.DatabaseName}");
                _logger.LogInformation($"Collection: {settings.CalendarCollectionName}");

                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);
            
                // This will create the collection if it doesn't exist
                _calendars = database.GetCollection<Calendar>(
                    settings.CalendarCollectionName);
                
                _logger.LogInformation("CalendarService initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize CalendarService");
                throw;
            }
        }

        public CalendarService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _calendars = database.GetCollection<Calendar>(mongoDBSettings.Value.CalendarCollectionName);
        }

        public async Task<List<Calendar>> GetAsync()
        {
            return await _calendars.Find(calendar => true).ToListAsync();
        }

        public async Task<Calendar> GetAsync(string id)
        {
            return await _calendars.Find<Calendar>(calendar => calendar.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Calendar calendar)
        {
            calendar.Id = null;
    
            // Explicit insert options
            var options = new InsertOneOptions { BypassDocumentValidation = false };
    
            await _calendars.InsertOneAsync(calendar, options);
    
            _logger.LogInformation($"Created event with ID: {calendar.Id}");
            
        }

        public async Task UpdateAsync(string id, Calendar calendarIn)
        {
            await _calendars.ReplaceOneAsync(calendar => calendar.Id == id, calendarIn);
        }

        public async Task RemoveAsync(string id)
        {
            await _calendars.DeleteOneAsync(calendarEvent => calendarEvent.Id == id);
        }

        public async Task<List<Calendar>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _calendars.Find(calendarEvent => 
                    calendarEvent.Start >= start && calendarEvent.End <= end)
                .ToListAsync();
        }
    }
}