// Services/ICalendarService.cs
using Calmy_Focus_App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Services
{
    public interface ICalendarService
    {
        Task<List<CalendarEvent>> GetAsync();
        Task<CalendarEvent?> GetAsync(string id);
        Task CreateAsync(CalendarEvent calendarEvent);
        Task UpdateAsync(string id, CalendarEvent calendarEvent);
        Task RemoveAsync(string id);
        Task<List<CalendarEvent>> GetByDateRangeAsync(DateTime start, DateTime end);
        
    }
}