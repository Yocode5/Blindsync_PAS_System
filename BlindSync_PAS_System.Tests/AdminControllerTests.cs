using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;
using Blindsync_PAS_System.Controllers;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlindSync_PAS_System.Tests
{
    public class AdminControllerTests
    {
        // Creates a fake DB for All Tests
        private AppDbContext GetInMemoryDbContext()
        {
            var option = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(option);
        }

        [Fact]
        public async Task AddNewUser_ReturnsError_WhenEmailAlreadyExists()
        {
            var context = GetInMemoryDbContext();
            var controller = new AdminController(context);

            // Seed the DB with a user 
            context.Users.Add(new User
            {
                Email = "test@uni.ac.lk",
                FirstName = "Existing",
                LastName = "User",
                Role = "Admin",
                PasswordHash = "fakehash",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            // Creating a user with an existing email
            var payload = new AddUserDTO
            {
                FirstName = "Thief",
                LastName = "Guy",
                Email = "test@uni.ac.lk", 
                Role = "Student",
                Password = "password123",
                StudentId = "STU999"
            };

            var result = await controller.AddNewUser(payload) as JsonResult;

            Assert.NotNull(result);

            var jsonString = JsonSerializer.Serialize(result.Value);
            var data = JsonDocument.Parse(jsonString).RootElement;

            Assert.False(data.GetProperty("success").GetBoolean());
            Assert.Equal("A user with this email already exists.", data.GetProperty("message").GetString());
        }

        [Fact]
        public async Task ToggleUserStatus_DeactivatesActiveUser_InsteadOfHardDelete()
        {
            var context = GetInMemoryDbContext();
            var controller = new AdminController(context);

            // Create a user who's status is currently active
            var testUser = new User
            {
                Email = "toggle@uni.ac.lk",
                FirstName = "Toggle",
                LastName = "Me",
                Role = "Student",
                PasswordHash = "fakehash",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(testUser);
            await context.SaveChangesAsync(); // Saving the test user to the fake DB

            var payload = new UserIdDTO { UserId = testUser.Id };
            var result = await controller.ToggleUserStatus(payload) as JsonResult;

            Assert.NotNull(result);

            var jsonString = JsonSerializer.Serialize(result.Value);
            var data = JsonDocument.Parse(jsonString).RootElement;

            Assert.True(data.GetProperty("success").GetBoolean());
            Assert.Equal("User Had been Deactivated", data.GetProperty("message").GetString());

            var userInDb = await context.Users.FindAsync(testUser.Id);
            Assert.False(userInDb.IsActive);
        }

        [Fact]
        public async Task AddNewUser_CreatesSupervisor_WithCorrectQuota()
        {
            var context = GetInMemoryDbContext();
            var controller = new AdminController(context);

            // Create a payload specifically for a new supervisor
            var payload = new AddUserDTO
            {
                FirstName = "Dr. Alan",
                LastName = "Grant",
                Email = "agrant@uni.ac.lk",
                Role = "Supervisor",
                Password = "securepassword123",
                SupervisorId = "SUP007",
                ProjectQuota = 5 
            };

            var result = await controller.AddNewUser(payload) as JsonResult;

            Assert.NotNull(result);

            var jsonString = JsonSerializer.Serialize(result.Value);
            var data = JsonDocument.Parse(jsonString).RootElement;

            Assert.True(data.GetProperty("success").GetBoolean());

            // Verify they were added to the Users table 
            var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "agrant@uni.ac.lk");
            Assert.NotNull(savedUser);
            Assert.Equal("Supervisor", savedUser.Role);

            // Verify they were added to the Supervisors table
            var savedSupervisor = await context.Supervisors.FirstOrDefaultAsync(s => s.UserId == savedUser.Id);
            Assert.NotNull(savedSupervisor);
            Assert.Equal("SUP007", savedSupervisor.SupervisorId);
            Assert.Equal(5, savedSupervisor.ProjectQuota);
        }

        [Fact]
        public async Task Overview_ReturnsCorrectViewModel_WithAggregatedData()
        {
            var context = GetInMemoryDbContext();
            var controller = new AdminController(context);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "bossadmin@uni.ac.lk"),
                    new Claim(ClaimTypes.Role, "Admin")
                }, "mock")
            );

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Seeding process
            var studentUser = new User { Email = "s1@uni.ac.lk", FirstName = "Sam", LastName = "Smith", Role = "Student", PasswordHash = "x", IsActive = true };
            var supUser = new User { Email = "sup1@uni.ac.lk", FirstName = "Dr. Bob", LastName = "Jones", Role = "Supervisor", PasswordHash = "x", IsActive = true };
            context.Users.AddRange(studentUser, supUser);
            await context.SaveChangesAsync();

            var student = new Student { StudentId = "STU001", UserId = studentUser.Id };
            var supervisor = new Supervisor { SupervisorId = "SUP001", UserId = supUser.Id, ProjectQuota = 5 };
            context.Students.Add(student);
            context.Supervisors.Add(supervisor);
            await context.SaveChangesAsync();

            // Adding one pending and one matched project 
            context.Projects.Add(new Project { Title = "Pending App", StudentId = student.Id, Status = ProjectStatus.Pending, Abstract = "Fake abstract data" });
            context.Projects.Add(new Project { Title = "Matched AI", StudentId = student.Id, SupervisorId = supervisor.Id, Status = ProjectStatus.Matched, Abstract = "Fake abstract data" });
            await context.SaveChangesAsync();

            var result = await controller.Overview() as ViewResult;

            Assert.NotNull(result);

            var model = Assert.IsType<AdminOverviewVM>(result.Model);

            Assert.Equal(1, model.TotalStudents);
            Assert.Equal(1, model.TotalSupervisors);
            Assert.Equal(1, model.TotalPending);
            Assert.Equal(1, model.TotalMatched);

            Assert.Equal(2, model.Projects.Count);

            Assert.Equal("bossadmin", controller.ViewBag.Username);
        }

        [Fact]
        public async Task ReassignSupervisor_UpdatesProject_AndChangesStatusToMatched()
        {
            var context = GetInMemoryDbContext();
            var controller = new AdminController(context);

            // Seeding a supervisor
            var supUser = new User { Email = "newsup@uni.ac.lk", FirstName = "Doc", LastName = "Brown", Role = "Supervisor", IsActive = true, PasswordHash = "x" };
            context.Users.Add(supUser);
            await context.SaveChangesAsync();

            var supervisor = new Supervisor { SupervisorId = "SUP999", UserId = supUser.Id, ProjectQuota = 5 };
            context.Supervisors.Add(supervisor);

            //Seeding a project that's Pending
            var project = new Project { Title = "Time Machine UI", Status = ProjectStatus.Pending, Abstract = "1.21 Gigawatts of pure science." };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var payload = new ReassignSupervisorDto
            {
                ProjectId = project.Id,
                SupervisorId = supervisor.Id,
            };

            var result = await controller.ReassignSupervisor(payload) as JsonResult;

            Assert.NotNull(result);

            var jsonString = System.Text.Json.JsonSerializer.Serialize(result.Value);
            var data = System.Text.Json.JsonDocument.Parse(jsonString).RootElement;

            Assert.True(data.GetProperty("success").GetBoolean());

            var updatedProject = await context.Projects.FindAsync(project.Id);

            Assert.Equal(supervisor.Id, updatedProject.SupervisorId);

            Assert.Equal(ProjectStatus.Matched, updatedProject.Status);
        }
    }
}