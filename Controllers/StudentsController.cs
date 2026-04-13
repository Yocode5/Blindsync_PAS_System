using Microsoft.AspNetCore.Mvc;

namespace Blindsync_PAS_System.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Dashboard()
        {
            return View(); 
        }
    }
}