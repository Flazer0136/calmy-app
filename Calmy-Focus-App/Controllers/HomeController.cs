using Calmy_Focus_App.Models;
using Calmy_Focus_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Calmy_Focus_App.Controllers   
{
    public class HomeController : Controller
    {
        private readonly INotesService _notesService;

        public HomeController(INotesService notesService)
        {
            _notesService = notesService;
        }

        public async Task<IActionResult> Index()
        {
            var notes = await _notesService.GetAsync();
            return View(notes);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(Note note)
        {
            try
            {
                await _notesService.CreateAsync(note);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNote(string id, Note note)
        {
            await _notesService.UpdateAsync(id, note);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNote(string id)
        {
            await _notesService.RemoveAsync(id);
            return RedirectToAction("Index");
        }
    }
}
