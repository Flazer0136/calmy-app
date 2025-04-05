using Calmy_Focus_App.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Calmy_Focus_App.Services;

public class MeditationService : IMeditationService
{
    private readonly IMongoCollection<MeditationSession> _sessions;
    private readonly IHabitService _habitService;
    private readonly List<string> _tracks = new() 
    { 
        "Ocean Waves", 
        "Forest Rain", 
        "Zen Garden",
        "White Noise"
    };

    public MeditationService(IOptions<MongoDBSettings> settings, IHabitService habitService)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _sessions = database.GetCollection<MeditationSession>("MeditationSessions");
        _habitService = habitService;
    }

    public async Task<List<MeditationSession>> GetUserSessionsAsync(string userId) =>
        await _sessions.Find(s => s.UserId == userId)
            .SortByDescending(s => s.StartTime)
            .Limit(50)
            .ToListAsync();

    public async Task<MeditationSession> StartSessionAsync(string userId, string trackName, int duration)
    {
        var session = new MeditationSession
        {
            UserId = userId,
            TrackName = trackName,
            DurationMinutes = duration,
            Completed = false
        };
        await _sessions.InsertOneAsync(session);
        return session;
    }

    public async Task CompleteSessionAsync(string sessionId)
    {
        var update = Builders<MeditationSession>.Update
            .Set(s => s.Completed, true)
            .Set(s => s.EndTime, DateTime.Now); // Now using the EndTime property

        await _sessions.UpdateOneAsync(
            Builders<MeditationSession>.Filter.Eq(s => s.Id, sessionId),
            update);

        await _habitService.LogMeditationCompletionAsync(sessionId);
    }
    
    public async Task DeleteSessionAsync(string id) // Renamed from RemoveAsync
    {
        await _sessions.DeleteOneAsync(s => s.Id == id);
    }

    public Task<List<string>> GetAvailableTracksAsync() => 
        Task.FromResult(_tracks);
}