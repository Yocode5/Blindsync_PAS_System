using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blindsync_PAS_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Overview()
        {
            ViewBag.UserName = User.Identity?.Name?.Split('@')[0] ?? "Guest";
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult ResearchAreas()
        {
            return View();
        }
    }
}
