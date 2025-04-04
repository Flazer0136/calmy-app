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
        private readonly IMongoCollection<Habit> _habitsCollection;

        public HabitService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _habitsCollection = database.GetCollection<Habit>(mongoDBSettings.Value.HabitsCollectionName);
        }

        public async Task<List<Habit>> GetAsync() =>
            await _habitsCollection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Habit habit) =>
            await _habitsCollection.InsertOneAsync(habit);

        public async Task RemoveAsync(string id) =>
            await _habitsCollection.DeleteOneAsync(h => h.Id == id);

        public async Task ToggleDailyCheck(string habitId)
        {
            var today = DateTime.UtcNow.Date;
            var filter = Builders<Habit>.Filter.Eq(h => h.Id, habitId);
            var habit = await _habitsCollection.Find(filter).FirstOrDefaultAsync();

            if (habit == null) return;

            var updateDefinition = habit.History.Contains(today)
                ? Builders<Habit>.Update
                    .Pull(h => h.History, today)
                    .Set(h => h.Streak, CalculateCurrentStreak(habit.History.Where(d => d != today).ToList()))
                : Builders<Habit>.Update
                    .Push(h => h.History, today)
                    .Set(h => h.Streak, CalculateCurrentStreak(habit.History.Concat(new[] { today }).ToList()));

            await _habitsCollection.UpdateOneAsync(filter, updateDefinition);
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