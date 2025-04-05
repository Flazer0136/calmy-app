// Services/ICalendarService.cs
using Calmy_Focus_App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Services
{
    public interface ICalendarService
    {
        Task<List<Calendar>> GetAsync();
        Task<Calendar> GetAsync(string id);
        Task CreateAsync(Calendar calendar);
        Task UpdateAsync(string id, Calendar calendar);
        Task RemoveAsync(string id);
        Task<List<Calendar>> GetByDateRangeAsync(DateTime start, DateTime end);
    }
}