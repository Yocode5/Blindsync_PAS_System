using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blindsync_PAS_System.Controllers
{
    [Authorize(Roles = "Admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

       public async Task<IActionResult> Overview()
        {
            //username from the email to display 
            ViewBag.UserName = User.Identity?.Name?.Split('@')[0] ?? "Guest";

            ViewBag.Supervisors = await _context.Supervisors
                .Include(s => s.UserAccount)
                .Select(s => new {
                    Id = s.Id, // Check if your primary key is 'Id' or 'SupervisorId'
                    FullName = s.UserAccount.FirstName + " " + s.UserAccount.LastName
                })
                .ToListAsync();

            var overviewData = new AdminOverviewVM
            {
                // Retrieve total counts for the top statistical cards
                TotalStudents = await _context.Students.CountAsync(), 
                TotalSupervisors = await _context.Supervisors.CountAsync(),
                TotalMatched = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.Matched),
                TotalPending = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.Pending),

                //Retrieve a list of recent projects to display in the overview data table
                Projects = await _context.Projects
                    .Include(p => p.Creator) // Join the Student (Creator) table
                        .ThenInclude(s => s.UserAccount) 
                    .Include(p => p.AssignedSupervisor) 
                        .ThenInclude(s => s.UserAccount) 
                    .Select(p => new ProjectOverviewDto
                    {
                        ProjectId = p.Id, 
                        ProjectTitle = p.Title,
                        StudentId = p.Creator.StudentId,                      
                        StudentName = p.Creator.UserAccount.FirstName + " " + p.Creator.UserAccount.LastName,                      
                        SupervisorName = p.AssignedSupervisor != null ? p.AssignedSupervisor.UserAccount.FirstName + " " + p.AssignedSupervisor.UserAccount.LastName : "Not Assigned",
                         Status = p.Status.ToString() 
                    })
                    .OrderByDescending(p => p.ProjectId) 
                    .Take(10) // Limit the table to 10 rows
                    .ToListAsync()
                    
            };           

            return View(overviewData);
        }
        

        public async Task<IActionResult> Users()
        {
            var loggedInEmail = User.Identity?.Name;

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loggedInEmail);

            ViewBag.FirstName = currentUser?.FirstName ?? "Admin";

            var vm = new ManageUsersVM();

            var students = await _context.Students.Include(s => s.UserAccount).ToListAsync();
            vm.Students = students.Select(s => new StudentVM
            {
                UserId = s.UserAccount.Id,                  
                FirstName = s.UserAccount.FirstName,        
                LastName = s.UserAccount.LastName,
                StudentId = s.StudentId,
                Name = $"{s.UserAccount.FirstName} {s.UserAccount.LastName}",
                Email = s.UserAccount.Email,
                IsActive = s.UserAccount.IsActive
            }).ToList();

            var supervisors = await _context.Supervisors.Include(s => s.UserAccount).ToListAsync();
            vm.Supervisors = supervisors.Select(s => new SupervisorVM
            {
                UserId= s.UserAccount.Id,
                FirstName = s.UserAccount.FirstName,
                LastName = s.UserAccount.LastName,
                SupervisorId = s.SupervisorId,
                Name = $"{s.UserAccount.FirstName} {s.UserAccount.LastName}",
                Email = s.UserAccount.Email,
                ProjectQuota = s.ProjectQuota,
                IsActive = s.UserAccount.IsActive
            }).ToList();

            var admins = await _context.Users.Where(u => u.Role == "Admin").ToListAsync();
            vm.Admins = admins.Select(a => new AdminVM
            {
                UserId = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                AdminId = $"ADM{a.Id:D3}",
                Name = $"{a.FirstName} {a.LastName}",
                Email = a.Email,
                IsActive = a.IsActive
            }).ToList();


            return View(vm);
        }
        public async Task<IActionResult> ResearchAreas()
        {
            var areas = await _context.ResearchAreas.OrderByDescending(a => a.Id).ToListAsync();
            return View(areas);
        }

        // Add a new Research Area
        [HttpPost]
        public async Task<IActionResult> AddResearchArea([FromBody] ResearchArea model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                TempData["ErrorMessage"] = "Research Area name cannot be empty.";
                return Json(new { success = false });
            }

            var exists = await _context.ResearchAreas
                .AnyAsync(a => a.Name.ToLower() == model.Name.ToLower());

            if (exists)
            {
                TempData["ErrorMessage"] = "This Research Area already exists.";
                return Json(new { success = false });
            }

            try
            {
                _context.ResearchAreas.Add(new ResearchArea
                {
                    Name = model.Name,
                    IsActive = true
                });

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Research Area added successfully!";
                return Json(new { success = true });
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while saving to the database.";
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteResearchArea(int id)
        {
            var area = await _context.ResearchAreas.FindAsync(id);
            if (area == null)
            {
            return Json(new { success = false, message = "Research Area not found." });
            }
            _context.ResearchAreas.Remove(area);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Research Area deleted successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] AddUserDTO model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Please fill in all required fields correctly." });

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return Json(new { success = false, message = "A user with this email already exists." });

            if (model.Role == "Student")
            {
                if (string.IsNullOrEmpty(model.StudentId))
                    return Json(new { success = false, message = "Student ID is required." });

                if (await _context.Students.AnyAsync(s => s.StudentId == model.StudentId))
                    return Json(new { success = false, message = "This Student ID is already registered." });
            }

            else if  (model.Role == "Supervisor")
            {
                if (string.IsNullOrWhiteSpace(model.SupervisorId))
                    return Json(new { success = false, message = "Supervisor ID is required." });

                if (await _context.Supervisors.AnyAsync(s => s.SupervisorId == model.SupervisorId))
                    return Json(new { success = false, message = "This Supervisor ID is already registered." });

                if ((model.ProjectQuota ?? 0) <= 0)
                    return Json(new { success = false, message = "Project Quota must be greater than zero." });
            }

            var hasher = new PasswordHasher<User>();

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Role = model.Role,
                PasswordHash = hasher.HashPassword(null, model.Password), 
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            if (model.Role == "Student")
            {
                _context.Students.Add(new Student { StudentId = model.StudentId, UserId = newUser.Id });
            }

            else if (model.Role == "Supervisor")
            {
                _context.Supervisors.Add(new Supervisor
                {
                    SupervisorId = model.SupervisorId,
                    ProjectQuota = model.ProjectQuota.Value,
                    UserId = newUser.Id
                });
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = $"{model.Role} added successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO model)
        {
            var user = await _context.Users.FindAsync(model.UserId);

            if (user == null)
                return Json(new { success = false, message = "User not found." });

            if (await _context.Users.AnyAsync(u => u.Email == model.Email && u.Id != model.UserId))
                return Json(new { success = false, message = "This email is already in use by another user." });

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, model.Password);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "User Updated Sucessfully" });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus([FromBody] UserIdDTO model)
        {
            var user = await _context.Users.FindAsync(model.UserId);
            if (user == null)
                return Json(new { success = false, message = "User not Found." });

            user.IsActive = !user.IsActive;

            await _context.SaveChangesAsync();

            string newStatusText = user.IsActive ? "Reactivated" : "Deactivated";
            return Json(new { success = true, message = $"User Had been {newStatusText}" });

        }
        [HttpPost]
        public async Task<IActionResult> ReassignSupervisor([FromBody] ReassignSupervisorDto model)
        {
            if (model == null || model.ProjectId == 0 || model.SupervisorId == 0)
            {
                return Json(new { success = false, message = "Invalid data received." });
            }            
            //Find the project by its ID
            var project = await _context.Projects.FindAsync(model.ProjectId);
            
            if (project == null)
            {
                return Json(new { success = false, message = "Project not found in the database." });
            }
            //Update the supervisor
            project.SupervisorId = model.SupervisorId;
            // If the project was "Pending", update it to "Matched" since a supervisor is now assigned
            if (project.Status == ProjectStatus.Pending)
             {
                 project.Status = ProjectStatus.Matched;
            }           
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Supervisor reassigned successfully!" });
            }  
        
        
    }
}