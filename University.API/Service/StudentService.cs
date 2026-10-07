
using Microsoft.EntityFrameworkCore;
using University.API.Data;
using University.API.DTOs;
using University.API.Models;

namespace University.API.Service
{
    public class StudentService
    {
        // Depencency Injection of the UDBC CLASS
        private readonly UniversityDBContext _context;

        //Creating instance of the Injection through the contructor
        public StudentService(UniversityDBContext context)
        {
            _context = context;
        }
        //Using Async Methods to access the specified models 
        //via the Instance of the Injected class to either
        //CREATE, READ, UPDATE, DELETE
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var Students = await _context.Students.ToListAsync();

            return Students;
        }

        public async Task<Student?> GetStudentAsync(int id)
        {

            var found = await _context.Students.FirstOrDefaultAsync(x => x.StudentID == id);
            if(found == null)
            { throw new Exception("404 Not Found"); }
            
            return found;

        }

        public async Task<Student> CreateStudentAsync(StudentDTO student)

        {

            if (student == null)
            {
                throw new Exception("Student details are required.");

            }

            var validstudent = _context.Students.Any(s => s.Email == student.Email);

            if (validstudent == true) {
                throw new Exception("409 Conflict");
            
            }

            Student response = new Student
            {
                StudentID = student.StudentID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Age = student.Age,
                Gender = student.Gender,
                DepartmentID = student.DepartmentID,
                EnrollmentYear = student.EnrollmentYear,
                Status = student.Status,
            };
             await _context.Students.AddAsync(response);

            
            //This will save the changes that the user
            //have done and update it in the database
           await _context.SaveChangesAsync();

            return response;
        }


        public async Task<Student> UpdateStudentAsync(int id, StudentDTO updatedstudent)
        {
            var updateStudent = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            if (updateStudent == null)
            {
                throw new Exception("404 Not Found");
               
            }
            updateStudent.FirstName = updatedstudent.FirstName;
            updateStudent.LastName = updatedstudent.LastName;
            updateStudent.Age = updatedstudent.Age;
            updateStudent.Email = updatedstudent.Email;
            await _context.SaveChangesAsync();


            return updateStudent;
        }

        public async Task<Student> DeleteStudentByIDAsync(int id)
        {

            var DeleteStudent = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            
            if (DeleteStudent==null)
            {
                throw new Exception("404 Not Found");
            }

            _context.Students.Remove(DeleteStudent);

            await _context.SaveChangesAsync();

            return DeleteStudent;
        }
    }
}
