using Microsoft.AspNetCore.Mvc;

namespace Blindsync_PAS_System.Controllers
{
    public class SupervisorsController : Controller
    {
        public IActionResult ReviewBoard()
        {
            return View(); // Looks for Views/Supervisors/ReviewBoard.cshtml
        }
    }
}