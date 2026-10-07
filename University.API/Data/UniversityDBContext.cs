
using Microsoft.EntityFrameworkCore;
using University.API.Models;


namespace University.API.Data
{
    public class UniversityDBContext : DbContext
    {

        //connecting the database instance by creating a constructor
        public UniversityDBContext(DbContextOptions options) : base(options)
        {
        }


        //creating objects for the tables/models so it can be reuseable 
        //and other classes will be able to access the nessesary information through 
        // dependency injection
        public DbSet<Student> Students {  get; set; }
        public DbSet<Department> Departments {  get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //using EF CORE to communicate to the database that
            //these classes represents the tables in the database
            modelBuilder.Entity<Student>()
                .ToTable("Student");
            modelBuilder.Entity<Department>()
                .ToTable("Department");

            //Showing the relationship between student and department being a 1 to many relationship
            modelBuilder.Entity<Student>().HasOne(Student => Student.Department)
                .WithMany(Department => Department.Students).HasForeignKey(Student => Student.DepartmentID);

           
        }
    }
}
