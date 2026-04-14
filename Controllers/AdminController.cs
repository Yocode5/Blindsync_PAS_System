using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blindsync_PAS_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Overview()
        {
            ViewBag.UserName = User.Identity?.Name?.Split('@')[0] ?? "Guest";

            var viewModel = new AdminOverviewVM
            {
                TotalStudents = 300,
                TotalMatched = 200,
                TotalSupervisors = 150,
                TotalPending = 100,
                Projects = new List<ProjectOverviewDto>
                {
                    new ProjectOverviewDto { ProjectId = 1, ProjectTitle = "AI Diagnosis", StudentId = "ST101", StudentName = "John Doe", SupervisorName = "Dr. Smith", Status = "Matched" },
                    new ProjectOverviewDto { ProjectId = 2, ProjectTitle = "Web Scraper", StudentId = "ST102", StudentName = "Jane Roe", SupervisorName = "Pending", Status = "Pending" },
                    new ProjectOverviewDto { ProjectId = 3, ProjectTitle = "Crypto Wallet", StudentId = "ST103", StudentName = "Sam Wilson", SupervisorName = "Dr. Silva", Status = "Under Review" }
                }
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Users()
        {
            ViewBag.UserName = User.Identity?.Name?.Split('@')[0] ?? "Guest";

            var vm = new ManageUsersVM();

            var students = await _context.Students.Include(s => s.UserAccount).ToListAsync();
            vm.Students = students.Select(s => new StudentVM
            {
                StudentId = s.StudentId,
                Name = $"{s.UserAccount.FirstName} {s.UserAccount.LastName}",
                Email = s.UserAccount.Email,
                IsActive = s.UserAccount.IsActive
            }).ToList();

            var supervisors = await _context.Supervisors.Include(s => s.UserAccount).ToListAsync();
            vm.Supervisors = supervisors.Select(s => new SupervisorVM
            {
                SupervisorId = s.SupervisorId,
                Name = $"{s.UserAccount.FirstName} {s.UserAccount.LastName}",
                Email = s.UserAccount.Email,
                ProjectQuota = s.ProjectQuota,
                IsActive = s.UserAccount.IsActive
            }).ToList();

            var admins = await _context.Users.Where(u => u.Role == "Admin").ToListAsync();
            vm.Admins = admins.Select(a => new AdminVM
            {
                AdminId = $"ADM{a.Id:D3}",
                Name = $"{a.FirstName} {a.LastName}",
                Email = a.Email,
                IsActive = a.IsActive
            }).ToList();


            return View(vm);
        }

        public IActionResult ResearchAreas()
        {
            return View();
        }
    }
}