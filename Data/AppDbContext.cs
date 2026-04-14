using Blindsync_PAS_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blindsync_PAS_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<ResearchArea> ResearchAreas { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Setting a proper seed date
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var hasher = new PasswordHasher<User>();

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "System",
                    LastName = "Admin",
                    Email = "admin@uni.ac.lk",
                    PasswordHash = hasher.HashPassword(null, "admin123"),
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = seedDate
                },

                new User
                {
                    Id = 2,
                    FirstName = "Test",
                    LastName = "Student",
                    Email = "student@uni.ac.lk",
                    PasswordHash = hasher.HashPassword(null, "student123"),
                    Role = "Student",
                    IsActive = true,
                    CreatedAt = seedDate
                },

                new User
                {
                    Id = 3,
                    FirstName = "Test",
                    LastName = "Supervisor",
                    Email = "supervisor@uni.ac.lk",
                    PasswordHash = hasher.HashPassword(null, "super123"),
                    Role = "Supervisor",
                    IsActive = true,
                    CreatedAt = seedDate
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, StudentId = "STU001", UserId = 2 }
            );

            modelBuilder.Entity<Supervisor>().HasData(
                new Supervisor { Id = 1, SupervisorId = "SUP001", ProjectQuota = 5, UserId = 3 }
            );
        }
    }
}
