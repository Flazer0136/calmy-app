using Calmy_Focus_App.Models;
using Calmy_Focus_App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Controllers
{
    public class HabitsController : Controller
    {
        private readonly IHabitService _habitService;

        public HabitsController(IHabitService habitService)
        {
            _habitService = habitService;
        }

        public async Task<IActionResult> Index()
        {
            var habits = await _habitService.GetAsync();
            ViewBag.Today = DateTime.Today.ToString("dddd, MMMM dd");
            return View(habits);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                await _habitService.CreateAsync(new Habit { Name = name });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCheck(string id)
        {
            await _habitService.ToggleDailyCheck(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _habitService.RemoveAsync(id);
            return RedirectToAction("Index");
        }

    }
}