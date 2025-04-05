// Controllers/CalendarController.cs
using Calmy_Focus_App.Models;
using Calmy_Focus_App.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(
            ICalendarService calendarService,
            ILogger<CalendarController> logger) // Add this parameter
        {
            _calendarService = calendarService;
            _logger = logger; // Store the logger
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Calendar calendar)
        {
            _logger.LogInformation("Creating calendar event");
            ModelState.Remove("Id");
        
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state");
                return BadRequest(ModelState);
            }

            try
            {
                await _calendarService.CreateAsync(calendar);
                return Json(new { 
                    success = true, 
                    id = calendar.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error creating event");
                return StatusCode(500, new { 
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _calendarService.RemoveAsync(id);
            return NoContent();
        }

        public async Task<IActionResult> Index()
        {
            var events = await _calendarService.GetAsync();
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, Calendar calendar)
        {
            if (ModelState.IsValid)
            {
                await _calendarService.UpdateAsync(id, calendar);
                return RedirectToAction("Index");
            }
            return View(calendar);
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents(DateTime start, DateTime end)
        {
            var events = await _calendarService.GetByDateRangeAsync(start, end);
            return Json(events);
        }
    }
}