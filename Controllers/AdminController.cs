using Microsoft.AspNetCore.Mvc;

namespace Blindsync_PAS_System.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
