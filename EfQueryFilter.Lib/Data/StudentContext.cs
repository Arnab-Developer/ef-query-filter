using EfQueryFilter.Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace EfQueryFilter.Lib.Data
{
    public class StudentContext : DbContext
    {
        public int SchoolId { get; private set; }

        public StudentContext(DbContextOptions<StudentContext> options, int schoolId = 0)
            : base(options)
        {
            SchoolId = schoolId;
        }

        public DbSet<Student>? Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Student>()
                .HasQueryFilter(s => s.SchoolId == SchoolId);
        }
    }
}
