using Microsoft.AspNetCore.Mvc;
<<<<<<< dilshan/feature/student-myproposal
using Blindsync_PAS_System.Models; 
=======
using Microsoft.EntityFrameworkCore;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
>>>>>>> master

namespace Blindsync_PAS_System.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;
        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
<<<<<<< dilshan/feature/student-myproposal
            return View();
        }

      
        public IActionResult Proposals()
        {
           
            var myProjects = new List<Project>();

            return View(myProjects);
=======
            var userEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Home");
            }

            var student = _context.Users.FirstOrDefault(s => s.Email == userEmail);

            if (student == null)
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.ResearchAreas = _context.ResearchAreas
                .Select(r => new { r.Id, r.Name })
                .ToList();

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProposal(string Title, int ResearchAreaId, string TechStack, string Abstract)
        {
            var userEmail = User.Identity?.Name;

            var userId = _context.Users
                .Where(u => u.Email == userEmail)
                .Select(u => u.Id)
                .FirstOrDefault();

            var student = _context.Students.FirstOrDefault(s => s.UserId == userId);

            if (student != null)
            {
                var newProject = new Project
                {
                    Title = Title,
                    ResearchAreaId = ResearchAreaId,
                    Abstract = Abstract,
                    StudentId = student.Id, 

                    TechStack = string.IsNullOrWhiteSpace(TechStack)
                        ? new List<string>()
                        : TechStack.Split(',').Select(t => t.Trim()).ToList(),

                    CreatedAt = DateTime.Now,
                    Status = ProjectStatus.Pending
                };

                _context.Projects.Add(newProject);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Proposal created successfully!";
            }

            return RedirectToAction("Dashboard");
>>>>>>> master
        }
    }
}