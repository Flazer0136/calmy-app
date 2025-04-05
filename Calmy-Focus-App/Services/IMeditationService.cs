using Calmy_Focus_App.Models;

namespace Calmy_Focus_App.Services;


public interface IMeditationService
{
    Task<List<MeditationSession>> GetUserSessionsAsync(string userId);
    Task<MeditationSession> StartSessionAsync(string userId, string trackName, int duration);
    Task CompleteSessionAsync(string sessionId);
    Task<List<string>> GetAvailableTracksAsync();
    Task DeleteSessionAsync(string id); // Changed from RemoveAsync to match controller
}