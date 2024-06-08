using DataAccess.Configuration;
using Microsoft.EntityFrameworkCore;
using IntroTask.Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Student>? Students { get; set; }
        public DbSet<Teacher>? Teachers { get; set; }
        public DbSet<Course>? Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseConfiguration).Assembly);
        }
    }
}
