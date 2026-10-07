using Microsoft.AspNetCore.Mvc;
using University.API.DTOs;
using University.API.Models;
using University.API.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace University.API.Controllers
{
    //Tells ASP.NET Core this is an API controller and enables
    //API-specific behavior such as automatic model validation.
      [ApiController]
    //Specifying the ROUTE through which the API would run through
    [Route("api/[controller]")]

    //Make sure when you extend the class it should be
    //ControllerBase this will define your rest API
    public class StudentController : ControllerBase
    {
        //Depenency Injection of the Student Service class
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        //Defines what the API endpoint is going to do 
        [HttpGet]
        public async Task <IActionResult> GetStudents()
        {
        var Student=  await _studentService.GetAllStudentsAsync();
            return Ok(Student);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentByID(int id)
        {
            try
            {
                var Student = await _studentService.GetStudentAsync(id);

                return Ok(Student);
            }
            catch
            {
                return NotFound("Student with this id does not exist");
            }
        }
        [HttpPost]
        public async Task <IActionResult> CreateStudent(StudentDTO student )
        {
            await _studentService.CreateStudentAsync(student);
            return Ok(student);
        }
        [HttpPut]
        public async Task< IActionResult> UpdateStudent(int id,StudentDTO UpdatedStudent)
        {
           var Updatedstudent=  await _studentService.UpdateStudentAsync(id, UpdatedStudent);
            return Ok(Updatedstudent);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var deletestudent = await _studentService.DeleteStudentByIDAsync(id);

                return Ok(deletestudent);
            }
            catch
            {
                return NotFound("student was not found");
            }
        }
        // NOTE:
        // Async operations usually propagate through the layers.
        // If the service performs an asynchronous database operation,
        // the controller that calls the service should normally also be async
        // so it can await the Task without blocking the thread.
    }
}
