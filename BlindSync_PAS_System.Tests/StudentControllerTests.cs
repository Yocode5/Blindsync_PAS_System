using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blindsync_PAS_System.Controllers;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlindSync_PAS_System.Tests
{
    public class StudentControllerTests
    {
        // Creates a fresh, fake DB for every test
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        // Helper method to fake a "Logged In" user
        private StudentsController GetControllerWithLoggedInUser(AppDbContext context, string email)
        {
            var controller = new StudentsController(context);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, email),
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            controller.TempData = new TempDataDictionary(controller.HttpContext, Moq.Mock.Of<ITempDataProvider>());

            return controller;
        }

        [Fact]
        public async Task CreateProposal_SavesNewProjectToDatabase_WhenValid()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();

            var fakeUser = new User { Id = 1, Email = "student@uni.ac.lk", Role = "Student", PasswordHash = "hash", FirstName = "John", LastName = "Doe" };
            var fakeStudent = new Student { Id = 1, UserId = 1, StudentId = "STU001" };
            var fakeArea = new ResearchArea { Id = 10, Name = "Data Science" };

            context.Users.Add(fakeUser);
            context.Students.Add(fakeStudent);
            context.ResearchAreas.Add(fakeArea);
            await context.SaveChangesAsync();

            var controller = GetControllerWithLoggedInUser(context, "student@uni.ac.lk");

            var payload = new ProposalVM
            {
                Title = "New Data Science Project",
                ResearchAreaId = 10,
                Abstract = "Testing the creation logic",
                TechStack = "C#"
            };

            // ACT
            var result = await controller.CreateProposal(payload);

            // ASSERT
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dashboard", redirectResult.ActionName);

            var savedProject = await context.Projects.FirstOrDefaultAsync();
            Assert.NotNull(savedProject);
            Assert.Equal("New Data Science Project", savedProject.Title);
            Assert.Equal(ProjectStatus.Pending, savedProject.Status); 
        }

        [Fact]
        public async Task WithdrawProposal_UpdatesStatusToWithdrawn_WhenUserOwnsProject()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();

            var fakeUser = new User { Id = 2, Email = "owner@uni.ac.lk", Role = "Student", PasswordHash = "hash", FirstName = "Owner", LastName = "Guy" };
            var fakeStudent = new Student { Id = 2, UserId = 2, StudentId = "STU002" };

            var project = new Project
            {
                Id = 100,
                Title = "My DS Project",
                Abstract = "This is a fake abstract.",
                StudentId = 2,
                Status = ProjectStatus.Pending,
                Creator = fakeStudent 
            };

            context.Users.Add(fakeUser);
            context.Students.Add(fakeStudent);
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var controller = GetControllerWithLoggedInUser(context, "owner@uni.ac.lk");

            // ACT 
            var result = await controller.WithdrawProposal(100);

            // ASSERT 
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dashboard", redirectResult.ActionName);

            var dbProject = await context.Projects.FindAsync(100);
            Assert.Equal(ProjectStatus.Withdrawn, dbProject.Status); 
        }

        [Fact]
        public async Task WithdrawProposal_DoesNotWithdraw_WhenUserDoesNotOwnProject()
        {
            // ARRANGE 
            var context = GetInMemoryDbContext();

            var innocentUser = new User { Id = 3, Email = "innocent@uni.ac.lk", Role = "Student", PasswordHash = "hash", FirstName = "Innocent", LastName = "Student" };
            var innocentStudent = new Student { Id = 3, UserId = 3, StudentId = "STU003", UserAccount = innocentUser };
            var project = new Project { Id = 101, Title = "Safe Project", Abstract = "This is a fake abstract.", StudentId = 3, Status = ProjectStatus.Pending, Creator = innocentStudent };

            var hackerUser = new User { Id = 4, Email = "hacker@uni.ac.lk", Role = "Student", PasswordHash = "hash", FirstName = "Hacker", LastName = "Student" };
            var hackerStudent = new Student { Id = 4, UserId = 4, StudentId = "STU004", UserAccount = hackerUser };

            context.Users.AddRange(innocentUser, hackerUser);
            context.Students.AddRange(innocentStudent, hackerStudent);
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var controller = GetControllerWithLoggedInUser(context, "hacker@uni.ac.lk");

            // ACT 
            var result = await controller.WithdrawProposal(101);

            // ASSERT
            var dbProject = await context.Projects.FindAsync(101);

            Assert.Equal(ProjectStatus.Pending, dbProject.Status);
        }

        [Fact]
        public async Task EditProposal_UpdatesProject_WhenDataIsValid()
        {
            // ARRANGE
            var context = GetInMemoryDbContext();

            var project = new Project
            {
                Id = 50,
                Title = "Old Title",
                Abstract = "Old Abstract",
                ResearchAreaId = 1,
                TechStack = "Old Tech"
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var controller = new StudentsController(context); 
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Moq.Mock.Of<ITempDataProvider>());

            var payload = new ProposalVM
            {
                Id = "50",
                Title = "Brand New Title",
                Abstract = "Brand New Abstract",
                ResearchAreaId = 2,
                TechStack = "C#, React"
            };

            // ACT 
            var result = await controller.EditProposal(payload) as JsonResult;

            // ASSERT
            Assert.NotNull(result);

            var jsonString = System.Text.Json.JsonSerializer.Serialize(result.Value);
            var data = System.Text.Json.JsonDocument.Parse(jsonString).RootElement;
            Assert.True(data.GetProperty("success").GetBoolean());

            var updatedProject = await context.Projects.FindAsync(50);
            Assert.Equal("Brand New Title", updatedProject.Title);
            Assert.Equal("Brand New Abstract", updatedProject.Abstract);
        }
    }
}