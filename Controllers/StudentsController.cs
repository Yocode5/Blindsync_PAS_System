using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;

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
            var userEmail = User.Identity?.Name;

            var student = _context.Students
                .Include(s => s.UserAccount) 
                .Include(s => s.Projects)
                    .ThenInclude(p => p.Area)
                .Include(s => s.Projects)
                    .ThenInclude(p => p.AssignedSupervisor)
                        .ThenInclude(sup => sup.UserAccount)
                .FirstOrDefault(s => s.UserAccount.Email == userEmail);

            if (student == null) return RedirectToAction("Login", "Home");

            var activeProject = student.Projects
                .FirstOrDefault(p => p.Status != ProjectStatus.Withdrawn);

            ViewBag.ActiveProject = activeProject;

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
                    TechStack = TechStack ?? "",
                    CreatedAt = DateTime.Now,
                    Status = ProjectStatus.Pending
                };

                _context.Projects.Add(newProject);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Proposal created successfully!";
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawProposal(int projectId)
        {
            var userEmail = User.Identity?.Name;

            var project = await _context.Projects
                .Include(p => p.Creator)
                    .ThenInclude(s => s.UserAccount)
                .FirstOrDefaultAsync(p => p.Id == projectId && p.Creator.UserAccount.Email == userEmail);

            if (project != null)
            {
                project.Status = ProjectStatus.Withdrawn;

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Proposal withdrawn successfully.";
            }

            return RedirectToAction("Dashboard");
        }
        
        public IActionResult Proposals()
        {
           
            var myProjects = new List<Project>();
        
            return View(myProjects);
        }
    }
}
