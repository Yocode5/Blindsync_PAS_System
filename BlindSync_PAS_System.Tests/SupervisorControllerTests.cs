using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Blindsync_PAS_System.Controllers;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.ViewModels;
using Blindsync_PAS_System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlindSync_PAS_System.Tests
{
    public class SupervisorControllerTests
    {
        // Setup in memory DB
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;
            return new AppDbContext(options);
        }

        // helper to mock logged in supervisor
        private void SetMockUser(SupervisorsController controller, string email)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, "Supervisor")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            
            controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                controller.HttpContext,
                Moq.Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());
        }

        [Fact]
        public async Task ReviewBoard_ReturnsOnlyPendingProjects_InViewModel()
        {
            var context = GetInMemoryDbContext();
            var controller = new SupervisorsController(context);
            var supEmail = "dr.doc@uni.ac.lk";

            SetMockUser(controller, supEmail);

            // Seed required Research Area
            var testArea = new ResearchArea { Name = "Test Area", IsActive = true };
            context.ResearchAreas.Add(testArea);

            // Seed supervisor
            var userAcc = new User { Email = supEmail, FirstName = "Doc", LastName = "Brown", Role = "Supervisor", PasswordHash = "x", IsActive = true };
            context.Users.Add(userAcc);
            await context.SaveChangesAsync();

            var supervisor = new Supervisor { SupervisorId = "SUP001", UserId = userAcc.Id, ProjectQuota = 5, UserAccount = userAcc };
            context.Supervisors.Add(supervisor);

            // Seed a student
            var studentUser = new User { Email = "stu1@uni.ac.lk", FirstName = "S", LastName = "T", Role = "Student", PasswordHash = "x" };
            context.Users.Add(studentUser);
            await context.SaveChangesAsync();

            var student = new Student { StudentId = "STU1", UserId = studentUser.Id, UserAccount = studentUser };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            // Seed one pending and matched project (WITH Student and Area)
            context.Projects.Add(new Project { Title = "Pending Thesis", Status = ProjectStatus.Pending, Abstract = "Needs review", ResearchAreaId = testArea.Id, StudentId = student.Id });
            context.Projects.Add(new Project { Title = "Already Matched", Status = ProjectStatus.Matched, Abstract = "Done deal", SupervisorId = supervisor.Id, ResearchAreaId = testArea.Id, StudentId = student.Id });
            await context.SaveChangesAsync();

            var result = await controller.ReviewBoard() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<SupervisorDashboardViewModel>(result.Model);

            // Should only grab the 1 Pending Project
            Assert.Single(model.AlignedProjects);
            Assert.Equal("Pending Thesis", model.AlignedProjects.First().Title);
            Assert.Equal("Doc", model.FirstName);
        }

        [Fact]
        public async Task AcceptProject_UpdatesStatusToMatched_AndAssignsSupervisor()
        {
            var context = GetInMemoryDbContext();
            var controller = new SupervisorsController(context);
            var supEmail = "mocksuper@uni.ac.lk";

            SetMockUser(controller, supEmail);

            // Seed Supervisor
            var userAcc = new User { Email = supEmail, FirstName = "Stephen", LastName = "Strange", Role = "Supervisor", PasswordHash = "x", IsActive = true };
            context.Users.Add(userAcc);
            await context.SaveChangesAsync();

            var supervisor = new Supervisor { SupervisorId = "SUP002", UserId = userAcc.Id, ProjectQuota = 3, UserAccount = userAcc };
            context.Supervisors.Add(supervisor);
            await context.SaveChangesAsync();

            // Seed a pending project
            var project = new Project { Title = "Multiverse UI", Status = ProjectStatus.Pending, Abstract = "Portals" };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var result = await controller.AcceptProject(project.Id) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            // Verify database changes
            var updatedProject = await context.Projects.FindAsync(project.Id);
            Assert.Equal(ProjectStatus.Matched, updatedProject.Status);
            Assert.Equal(supervisor.Id, updatedProject.SupervisorId);
            Assert.NotNull(updatedProject.AssignedAt);
        }

        [Fact]
        public async Task MyMatches_ReturnsOnlyProjects_AssignedToLoggedInSupervisor()
        {
            var context = GetInMemoryDbContext();
            var controller = new SupervisorsController(context);
            var myEmail = "myemail@uni.ac.lk";

            SetMockUser(controller, myEmail);

            // Seed required Research Area
            var testArea = new ResearchArea { Name = "Test Area", IsActive = true };
            context.ResearchAreas.Add(testArea);

            // Seed a Student
            var studentUser = new User { Email = "stu2@uni.ac.lk", FirstName = "S", LastName = "T", Role = "Student", PasswordHash = "x" };
            context.Users.Add(studentUser);
            await context.SaveChangesAsync();

            var student = new Student { StudentId = "STU2", UserId = studentUser.Id, UserAccount = studentUser };
            context.Students.Add(student);

            // Seed a Logged-in supervisor
            var myUser = new User { Email = myEmail, FirstName = "My", LastName = "Name", Role = "Supervisor", PasswordHash = "x" };
            context.Users.Add(myUser);
            await context.SaveChangesAsync();
            var mySupervisor = new Supervisor { SupervisorId = "SUP003", UserId = myUser.Id, ProjectQuota = 5, UserAccount = myUser };
            context.Supervisors.Add(mySupervisor);
            await context.SaveChangesAsync();

            // Seed a different supervisor
            var otherUser = new User { Email = "other@uni.ac.lk", FirstName = "Other", LastName = "Guy", Role = "Supervisor", PasswordHash = "x" };
            context.Users.Add(otherUser);
            await context.SaveChangesAsync();
            var otherSupervisor = new Supervisor { SupervisorId = "SUP004", UserId = otherUser.Id, ProjectQuota = 5, UserAccount = otherUser };
            context.Supervisors.Add(otherSupervisor);
            await context.SaveChangesAsync();

            // Seed projects WITH Area and Student attached
            context.Projects.Add(new Project { Title = "A Student's Project", Status = ProjectStatus.Matched, SupervisorId = mySupervisor.Id, Abstract = "Abstract", ResearchAreaId = testArea.Id, StudentId = student.Id });
            context.Projects.Add(new Project { Title = "Another Student's Project", Status = ProjectStatus.Matched, SupervisorId = otherSupervisor.Id, Abstract = "Abstract", ResearchAreaId = testArea.Id, StudentId = student.Id });
            await context.SaveChangesAsync();

            var result = controller.MyMatches() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Project>>(result.Model);

            Assert.Single(model);
            Assert.Equal("A Student's Project", model.First().Title);
        }
    }
}