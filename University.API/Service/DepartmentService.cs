
using Mapster;
using Microsoft.EntityFrameworkCore;

using University.API.Data;
using University.API.DTOs;
using University.API.Models;
namespace University.API.Service {
    public class DepartmentService
    {
        private readonly UniversityDBContext _context;

        public DepartmentService(UniversityDBContext context)
        {
            _context = context;
        }
       
        public async Task<List<DepartmentDTO>> DisplayDepartmentAsync()
        {
            var department = await _context.Departments.ToListAsync();

            List<DepartmentDTO> response =  department.Select(x => new DepartmentDTO
            {
                DepartmentID=x.DepartmentID,
                DepartmentName=x.DepartmentName,
                Building=x.Building,
                Budget=x.Budget,
                Status=x.Status,

            }).ToList();
       
            

            return response;
        }

        public async Task <Department?> GetDepartmentByIDAsync(int id)
        {
            var search= await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentID==id);

            if (search == null) {
                throw new Exception("400 Bad Request");
            }
            return search;
        }
       
        public async Task <DepartmentDTO> CreateDepartmentAsync(DepartmentDTO department)
        {

        Department response = new Department {
                  
                  DepartmentName=department.DepartmentName,
                Building=department.Building,
              };
            
          
            var validateDepartment = await _context.Departments.AnyAsync(d => d.Building == department.Building);
           

            if (validateDepartment==true) {
                throw new Exception("409 Conflict");
            }
            

            await _context.Departments.AddAsync(response);
            await _context.SaveChangesAsync();

            DepartmentDTO result = new DepartmentDTO
            {
                DepartmentID=response.DepartmentID,
                DepartmentName = response.DepartmentName,
                Building = response.Building,
            };
            return result ;
        }

        public async Task<Department> UpdateDepartmentAsync(int id, DepartmentDTO updatedDepartment)
        {
            var validateDepartment = await _context.Departments.FirstOrDefaultAsync(s => s.DepartmentID == id);
            if (validateDepartment == null)
            {
                throw new Exception("400 Bad Request");

            }

            validateDepartment.DepartmentName = updatedDepartment.DepartmentName;
            validateDepartment.Building = updatedDepartment.Building;
            await _context.SaveChangesAsync();


            return validateDepartment;
        }

        public async Task<Department> DeleteDepartmentAsync(int id)
        {
            var deleteDepartment= await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentID == id);
            if (deleteDepartment == null) {
                throw new Exception("404 Not Found");
            }
           

            _context.Departments.Remove(deleteDepartment);
           await _context.SaveChangesAsync();

            return deleteDepartment;
        }
    }

}