using Calmy_Focus_App.Models;
using Calmy_Focus_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Calmy_Focus_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notes = await _notesService.GetAsync();
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var note = await _notesService.GetAsync(id);
            return note == null ? NotFound() : Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Note note)
        {
            await _notesService.CreateAsync(note);
            return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Note note)
        {
            await _notesService.UpdateAsync(id, note);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _notesService.RemoveAsync(id);
            return NoContent();
        }
    }
}