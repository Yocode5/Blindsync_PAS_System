using Microsoft.AspNetCore.Mvc;

namespace Blindsync_PAS_System.Controllers
{
    public class SupervisorsController : Controller
    {
        public IActionResult ReviewBoard()
        {
            return View();
        }

        public IActionResult MyMatches()
        {
            return View();
        }
    }
}