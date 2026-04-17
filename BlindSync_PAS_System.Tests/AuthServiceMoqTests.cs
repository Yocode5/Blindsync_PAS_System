using Moq;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Services;
using Blindsync_PAS_System.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BlindSync_PAS_System.Tests
{
    public class AuthServiceMoqTests
    {
        [Fact]
        public void ValidateUser_ReturnsRole_WhenCredentialsAreCorrect_UsingMoq()
        {
            var hasher = new PasswordHasher<User>();
            var fakeUser = new User { Email = "valid@uni.ac.lk", Role = "Admin", IsActive = true };
            fakeUser.PasswordHash = hasher.HashPassword(fakeUser, "CorrectPassword123");

            var data = new List<User> { fakeUser }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var authService = new AuthService(mockContext.Object);
            var result = authService.ValidateUser(new LoginVM { Email = "valid@uni.ac.lk", Password = "CorrectPassword123" });

            Assert.Equal("Admin", result);
        }
    }
}
