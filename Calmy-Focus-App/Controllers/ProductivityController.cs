using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calmy_Focus_App.Controllers
{
    // This attribute ensures that all actions in this controller require authentication.
    [Authorize]
    public class ProductivityController : Controller
    {
        // GET: /Productivity/Timer
        public IActionResult Timer()
        {
            return View();
        }
    }
}