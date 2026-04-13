using Microsoft.AspNetCore.Mvc;
using Blindsync_PAS_System.Models; 

namespace Blindsync_PAS_System.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

      
        public IActionResult Proposals()
        {
           
            var myProjects = new List<Project>();

            return View(myProjects);
        }
    }
}