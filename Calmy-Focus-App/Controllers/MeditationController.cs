using Calmy_Focus_App.Models;
using Calmy_Focus_App.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Controllers
{
    public class MeditationController : Controller
    {
        private readonly IMeditationService _meditationService;
        private readonly IHabitService _habitService;

        public MeditationController(IMeditationService meditationService, IHabitService habitService)
        {
            _meditationService = meditationService;
            _habitService = habitService;
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await _meditationService.GetUserSessionsAsync("current-user-id");
            var tracks = await _meditationService.GetAvailableTracksAsync();
            var currentSession = sessions.FirstOrDefault(s => !s.Completed);
    
            var viewModel = new MeditationViewModel
            {
                Sessions = sessions.OrderByDescending(s => s.StartTime).ToList(),
                AvailableTracks = tracks,
                CurrentSession = currentSession
            };
    
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartSession(string trackName, int durationMinutes)
        {
            if (!string.IsNullOrWhiteSpace(trackName))
            {
                await _meditationService.StartSessionAsync("current-user-id", trackName, durationMinutes);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteSession(string sessionId)
        {
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                await _meditationService.CompleteSessionAsync(sessionId);
                await _habitService.LogMeditationCompletionAsync(sessionId);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSession(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                await _meditationService.DeleteSessionAsync(id); // Updated method name
            }
            return RedirectToAction("Index");
        }
    }
}