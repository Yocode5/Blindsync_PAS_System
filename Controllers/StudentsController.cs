using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.ViewModels; 

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
        public async Task<IActionResult> CreateProposal(ProposalVM model) 
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
                    Title = model.Title,
                    ResearchAreaId = model.ResearchAreaId.Value, 
                    Abstract = model.Abstract,
                    StudentId = student.Id,
                    TechStack = model.TechStack ?? "",
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
            var userEmail = User.Identity?.Name;

            var myProposals = _context.Projects
                .Include(p => p.Area)
                .Where(p => p.Creator.UserAccount.Email == userEmail)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(myProposals);
        }

        [HttpPost]
        public async Task<IActionResult> EditProposal(ProposalVM model) 
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(" ", errors) });
            }

            if (!int.TryParse(model.Id, out int projectId))
            {
                return Json(new { success = false, message = "Invalid project ID." });
            }

            var project = await _context.Projects.FindAsync(projectId);

            if (project == null)
            {
                return Json(new { success = false, message = "Project not found." });
            }

            project.Title = model.Title ?? "";
            project.ResearchAreaId = model.ResearchAreaId.Value;
            project.TechStack = model.TechStack ?? "";
            project.Abstract = model.Abstract ?? "";

            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Proposal updated successfully!";

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while saving to the database." });
            }
        }
    }
}