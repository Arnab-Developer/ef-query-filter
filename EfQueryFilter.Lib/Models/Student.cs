namespace EfQueryFilter.Lib.Models
{
    public class Student : ISchool
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public int SchoolId { get; set; }

        public Student()
        {
            Name = string.Empty;
            Subject = string.Empty;
        }
    }
}
