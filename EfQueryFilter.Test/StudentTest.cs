using EfQueryFilter.Lib.Data;
using EfQueryFilter.Lib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EfQueryFilter.Test
{
    public class StudentTest : IDisposable
    {
        private readonly StudentContext _context;
        private bool disposedValue;

        public StudentTest()
        {
            var options = new DbContextOptionsBuilder<StudentContext>()
                .UseInMemoryDatabase("StudentDb")
                .Options;
            _context = new StudentContext(options);
            IEnumerable<Student> students = new List<Student>
                {
                    new Student { Id = 1, Name = "n1", Subject = "s1", SchoolId = 1 },
                    new Student { Id = 2, Name = "n2", Subject = "s2", SchoolId = 1 },
                    new Student { Id = 3, Name = "n3", Subject = "s3", SchoolId = 2 },
                    new Student { Id = 4, Name = "n4", Subject = "s4", SchoolId = 2 }
                };
            _context.Students?.AddRange(students);
            _context.SaveChanges();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void VerifyThatSchoolCanNotGetOtherSchoolsData(int schoolId)
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<StudentContext>()
                .UseInMemoryDatabase("StudentDb")
                .Options;
            using var context = new StudentContext(options, schoolId);
            context.Database.EnsureCreated();

            // Act.
            List<Student> students = context.Students
                .Where(s => s.Name == "n4")
                .OrderBy(s => s.Name)
                .ToList();

            // Assert.
            if (schoolId == 1)
            {
                Assert.Empty(students);
            }
            if (schoolId == 2)
            {
                Assert.Single(students);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Database.EnsureDeleted();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
