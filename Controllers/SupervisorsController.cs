using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Blindsync_PAS_System.ViewModels;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blindsync_PAS_System.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class SupervisorsController : Controller
    {
        private readonly AppDbContext _context;

        public SupervisorsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ReviewBoard()
        {
            var userEmail = User.Identity?.Name;
            var supervisor = await _context.Supervisors
                .Include(s => s.UserAccount)
                .FirstOrDefaultAsync(s => s.UserAccount.Email == userEmail);

            if (supervisor == null) return RedirectToAction("Login", "Home");

            var expertise = new List<string> { "Machine Learning", "Cloud Computing", "IoT" };

            var pendingProjects = await _context.Projects
                .Include(p => p.Area)
                .Where(p => p.Status == ProjectStatus.Pending)
                .ToListAsync();

            var allResearchAreas = pendingProjects
                .Where(p => p.Area != null)
                .Select(p => p.Area.Name)
                .Distinct()
                .ToList();

            var model = new SupervisorDashboardViewModel
            {
                FirstName = supervisor.UserAccount?.FirstName ?? "Supervisor",
                ExpertiseAreas = expertise,
                AvailableResearchAreas = allResearchAreas,
                AlignedProjects = pendingProjects.Select(p => new ProjectProposalViewModel
                {
                    ProjectId = p.Id,
                    Title = p.Title,
                    ResearchArea = p.Area?.Name ?? "N/A",
                    TechStack = string.IsNullOrWhiteSpace(p.TechStack)
                        ? new List<string>()
                        : p.TechStack.Split(',').Select(t => t.Trim()).ToList(),
                    AbstractText = p.Abstract
                }).ToList()
            };

            return View(model);
        }

        public IActionResult MyMatches()
        {
            var userEmail = User.Identity?.Name;
            var supervisor = _context.Supervisors
                .Include(s => s.UserAccount)
                .FirstOrDefault(s => s.UserAccount.Email == userEmail);

            if (supervisor == null) return RedirectToAction("Login", "Home");

            var matchedProjects = _context.Projects
                .Include(p => p.Area)
                .Include(p => p.Creator)
                    .ThenInclude(c => c.UserAccount)
                .Where(p => p.SupervisorId == supervisor.Id && p.Status == ProjectStatus.Matched)
                .OrderByDescending(p => p.AssignedAt)
                .ToList();

            ViewBag.FirstName = supervisor.UserAccount.FirstName;

            return View(matchedProjects);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            var userEmail = User.Identity?.Name;
            var supervisor = await _context.Supervisors
                .Include(s => s.UserAccount)
                .FirstOrDefaultAsync(s => s.UserAccount.Email == userEmail);

            if (supervisor == null) return Unauthorized(new { message = "Unauthorized access" });

            project.Status = ProjectStatus.Matched;
            project.SupervisorId = supervisor.Id;
            project.AssignedAt = DateTime.UtcNow;

            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Project accepted successfully!" });
            }
            catch
            {
                return StatusCode(500, new { message = "Error updating database" });
            }
        }
    }
}