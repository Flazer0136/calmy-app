using Calmy_Focus_App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Services
{
    public interface IHabitService
    {
        Task<List<Habit>> GetAsync();
        Task CreateAsync(Habit habit);
        Task RemoveAsync(string id);
        Task ToggleDailyCheck(string habitId);
        Task LogMeditationCompletionAsync(string sessionId); 
    }
}