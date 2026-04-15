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
    }
}