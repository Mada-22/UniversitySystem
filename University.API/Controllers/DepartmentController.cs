using Microsoft.AspNetCore.Mvc;
using University.API.DTOs;
using University.API.Models;
using University.API.Service;

namespace University.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> DisplayDepartments()
        {
            var departments = await _departmentService.DisplayDepartmentAsync();
            return Ok(departments);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentByID(int id) {
            try
            {
                var department = await _departmentService.GetDepartmentByIDAsync(id);
                return Ok(department);
            }
            catch
            {
                return NotFound("Department doesnot exist");
            }
           
        }
        [HttpPost]
        public async Task< IActionResult> CreateDepartment(DepartmentDTO department)
        {
            try
            {
                var createdDepartment = await _departmentService.CreateDepartmentAsync(department);
                return Ok(department);
            }
            catch
            {
                return BadRequest("Department already exist");
            }
        }
        [HttpPut]
        public async Task< IActionResult> EditDepartment(int id , DepartmentDTO department) {
            try
            {
                var editDepartment = await _departmentService.UpdateDepartmentAsync(id, department);
                return Ok(editDepartment);
            }
            catch
            {
                return NotFound($"ID {id} does not exist");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var deleteDepartment = await _departmentService.DeleteDepartmentAsync(id);
                return Ok(deleteDepartment);
            }
            catch
            {
                return NotFound($"ID {id} does not exist");
            }
            
            }
    }
}
