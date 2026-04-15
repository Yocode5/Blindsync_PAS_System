using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.Services;
using Blindsync_PAS_System.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace BlindSync_PAS_System.Tests
{
    public class AuthServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public void ValidateUser_ReturnsRole_WhenCredentialsAreCorrect()
        {
            var context = GetInMemoryDbContext();
            var authService = new AuthService(context);
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Email = "valid@uni.ac.lk",
                FirstName = "John",
                LastName = "Doe",
                Role = "Admin", 
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = hasher.HashPassword(user, "CorrectPassword123");

            context.Users.Add(user);
            context.SaveChanges();

            var loginModel = new LoginVM { Email = "valid@uni.ac.lk", Password = "CorrectPassword123" };

            var result = authService.ValidateUser(loginModel);

            Assert.Equal("Admin", result);
        }

        [Fact]
        public void ValidateUser_ReturnsNull_WhenPasswordIsIncorrect()
        {
            var context = GetInMemoryDbContext();
            var authService = new AuthService(context);
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Email = "illegaluser@uni.ac.lk",
                FirstName = "Illegal",
                LastName = "User",
                Role = "Student",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = hasher.HashPassword(user, "SecurePass!");

            context.Users.Add(user);
            context.SaveChanges();

            var loginModel = new LoginVM { Email = "illegaluser@uni.ac.lk", Password = "WrongPassword!" };

            var result = authService.ValidateUser(loginModel);

            Assert.Null(result);
        }

        [Fact]
        public void ValidateUser_ReturnsNull_WhenUserIsDeactivated() {
            var context = GetInMemoryDbContext();
            var authService = new AuthService(context);
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Email = "inactive@uni.ac.lk",
                FirstName = "Test",
                LastName = "Sup",
                Role = "Supervisor",
                IsActive = false, 
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = hasher.HashPassword(user, "ValidPass123");

            context.Users.Add(user);
            context.SaveChanges();

            var loginModel = new LoginVM { Email = "inactive@uni.ac.lk", Password = "ValidPass123" };

            var result = authService.ValidateUser(loginModel);
            Assert.Null(result);
        }
    }
}
