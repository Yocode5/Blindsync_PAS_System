using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blindsync_PAS_System.ViewModels;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blindsync_PAS_System.Controllers 
{
    public class SupervisorsController : Controller
    {
        private readonly AppDbContext _context;

        public SupervisorsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ReviewBoard()
        {
            //Replace with the logged-in user's ID once Authentication is implemented
            int currentSupervisorId = 1; 

            var supervisor = await _context.Supervisors
                .Include(s => s.UserAccount)
                .FirstOrDefaultAsync(s => s.Id == currentSupervisorId);

            if (supervisor == null) return NotFound();

            //Fetch from the database once the Expertise table is added
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AcceptProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound(new { message = "Project not found" });
            }

            int currentSupervisorId = 1; 

            project.Status = ProjectStatus.Matched; 
            project.SupervisorId = currentSupervisorId;
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