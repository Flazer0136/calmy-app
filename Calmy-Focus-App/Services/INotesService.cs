using Calmy_Focus_App.Models;

namespace Calmy_Focus_App.Services
{
    public interface INotesService
    {
        Task<List<Note>> GetAsync();
        Task<Note?> GetAsync(string id);
        Task CreateAsync(Note note);
        Task UpdateAsync(string id, Note note);
        Task RemoveAsync(string id);
    }
}