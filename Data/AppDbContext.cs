using Microsoft.EntityFrameworkCore;
using Blindsync_PAS_System.Models;

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
    }
}
