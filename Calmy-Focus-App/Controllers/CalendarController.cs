// Controllers/CalendarController.cs
using Calmy_Focus_App.Models;
using Calmy_Focus_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Controllers
{
    [Authorize]  // This protects all actions in the controller.
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(
            ICalendarService calendarService,
            ILogger<CalendarController> logger)
        {
            _calendarService = calendarService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CalendarEvent calendarEvent)
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
                await _calendarService.CreateAsync(calendarEvent);
                return Json(new
                {
                    success = true,
                    id = calendarEvent.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error creating event");
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _calendarService.RemoveAsync(id);
                return Ok(new { success = true });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete error");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        public async Task<IActionResult> Index()
        {
            var events = await _calendarService.GetAsync();
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                await _calendarService.UpdateAsync(id, calendarEvent);
                return RedirectToAction("Index");
            }
            return View(calendarEvent);
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents(DateTime start, DateTime end)
        {
            var events = await _calendarService.GetByDateRangeAsync(start, end);
            return Json(events);
        }
    }
}
