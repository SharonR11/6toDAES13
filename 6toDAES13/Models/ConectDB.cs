using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace _6toDAES13.Models
{
    public class ConectDB: DbContext
    {
        //public ConectDB(DbContextOptions<ConectDB> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAB1504-27\\SQLEXPRESS; Initial Catalog=SharonDB; trustservercertificate=True;User Id=userSharon;Password=010203");
        }
    }
}
