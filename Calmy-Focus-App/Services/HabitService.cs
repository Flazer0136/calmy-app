using Calmy_Focus_App.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Services
{
    public class HabitService : IHabitService
    {
        private readonly IMongoCollection<Habit> _habits;
        private readonly IMongoCollection<MeditationSession> _meditationSessions;

        public HabitService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _habits = database.GetCollection<Habit>(mongoDBSettings.Value.HabitsCollectionName);
            _meditationSessions = database.GetCollection<MeditationSession>("MeditationSessions");
        }

        public async Task<List<Habit>> GetAsync() =>
            await _habits.Find(_ => true).ToListAsync();

        public async Task<Habit?> GetAsync(string id) =>
            await _habits.Find(h => h.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Habit habit) =>
            await _habits.InsertOneAsync(habit);

        public async Task RemoveAsync(string id) =>
            await _habits.DeleteOneAsync(h => h.Id == id);

        public async Task ToggleDailyCheck(string habitId)
        {
            var today = DateTime.UtcNow.Date;
            var habit = await _habits.Find(h => h.Id == habitId).FirstOrDefaultAsync();

            if (habit == null) return;

            var updateDefinition = habit.History.Contains(today)
                ? Builders<Habit>.Update
                    .Pull(h => h.History, today)
                    .Set(h => h.Streak, CalculateCurrentStreak(habit.History.Where(d => d != today).ToList()))
                : Builders<Habit>.Update
                    .Push(h => h.History, today)
                    .Set(h => h.Streak, CalculateCurrentStreak(habit.History.Concat(new[] { today }).ToList()));

            await _habits.UpdateOneAsync(
                Builders<Habit>.Filter.Eq(h => h.Id, habitId),
                updateDefinition);
        }

        public async Task LogMeditationCompletionAsync(string sessionId)
        {
            // Find or create the Meditation habit
            var habit = await _habits.Find(h => h.Name == "Meditation").FirstOrDefaultAsync();
            if (habit == null)
            {
                habit = new Habit { 
                    Name = "Meditation",
                    History = new List<DateTime>(),
                    Streak = 0
                };
                await _habits.InsertOneAsync(habit);
            }

            // Log completion if not already logged today
            var today = DateTime.UtcNow.Date;
            if (!habit.History.Contains(today))
            {
                await ToggleDailyCheck(habit.Id);
            }
        }
        
        private int CalculateCurrentStreak(List<DateTime> history)
        {
            if (history.Count == 0) return 0;

            var orderedDates = history
                .Select(d => d.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            int streak = 0;
            DateTime currentDate = DateTime.UtcNow.Date;

            foreach (var date in orderedDates)
            {
                if (date == currentDate.AddDays(-streak))
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }

            return streak;
        }
    }
}