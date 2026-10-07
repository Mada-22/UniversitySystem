using Microsoft.EntityFrameworkCore;
using University.API.Data;
using University.API.DTOs;
using University.API.Models;
using University.API.Service;

namespace University.Test
{
    public class DepartmentServiceTest
    {
        private UniversityDBContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<UniversityDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new UniversityDBContext(options);
        }


        // CREATE
        [Fact]
        public async Task CreateDepartmentAsync_ShouldCreateDepartment()
        {
            // Arrange
            using var context = CreateContext();

            var service = new DepartmentService(context);

            var departmentDTO = new DepartmentDTO
            {
                DepartmentName = "Information Technology",
                Building = "Building A"
            };

            // Act
            var result = await service.CreateDepartmentAsync(departmentDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Information Technology", result.DepartmentName);
            Assert.Equal("Building A", result.Building);

            Assert.True(result.DepartmentID > 0);
        }


        // CREATE - DUPLICATE BUILDING
        [Fact]
        public async Task CreateDepartmentAsync_ShouldThrowException_WhenBuildingAlreadyExists()
        {
            // Arrange
            using var context = CreateContext();

            context.Departments.Add(new Department
            {
                DepartmentID = 1,
                DepartmentName = "Information Technology",
                Building = "Building A"
            });

            await context.SaveChangesAsync();

            var service = new DepartmentService(context);

            var departmentDTO = new DepartmentDTO
            {
                DepartmentName = "Computer Science",
                Building = "Building A"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.CreateDepartmentAsync(departmentDTO));

            Assert.Equal("409 Conflict", exception.Message);
        }


        // GET BY ID
        [Fact]
        public async Task GetDepartmentByIDAsync_ShouldReturnDepartment()
        {
            // Arrange
            using var context = CreateContext();

            context.Departments.Add(new Department
            {
                DepartmentID = 1,
                DepartmentName = "Information Technology",
                Building = "Building A"
            });

            await context.SaveChangesAsync();

            var service = new DepartmentService(context);

            // Act
            var result = await service.GetDepartmentByIDAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.DepartmentID);
            Assert.Equal("Information Technology", result.DepartmentName);
            Assert.Equal("Building A", result.Building);
        }


        // GET BY ID - NOT FOUND
        [Fact]
        public async Task GetDepartmentByIDAsync_ShouldThrowException_WhenDepartmentDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();

            var service = new DepartmentService(context);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.GetDepartmentByIDAsync(999));

            Assert.Equal("400 Bad Request", exception.Message);
        }


        // GET ALL
        [Fact]
        public async Task DisplayDepartmentAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            using var context = CreateContext();

            context.Departments.AddRange(
                new Department
                {
                    DepartmentID = 1,
                    DepartmentName = "Information Technology",
                    Building = "Building A"
                },
                new Department
                {
                    DepartmentID = 2,
                    DepartmentName = "Finance",
                    Building = "Building B"
                }
            );

            await context.SaveChangesAsync();

            var service = new DepartmentService(context);

            // Act
            var result = await service.DisplayDepartmentAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal("Information Technology", result[0].DepartmentName);
            Assert.Equal("Finance", result[1].DepartmentName);
        }


        // GET ALL - EMPTY DATABASE
        [Fact]
        public async Task DisplayDepartmentAsync_ShouldReturnEmptyList_WhenNoDepartmentsExist()
        {
            // Arrange
            using var context = CreateContext();

            var service = new DepartmentService(context);

            // Act
            var result = await service.DisplayDepartmentAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        // UPDATE
        [Fact]
        public async Task UpdateDepartmentAsync_ShouldUpdateDepartment()
        {
            // Arrange
            using var context = CreateContext();

            context.Departments.Add(new Department
            {
                DepartmentID = 1,
                DepartmentName = "Information Technology",
                Building = "Building A"
            });

            await context.SaveChangesAsync();

            var service = new DepartmentService(context);

            var updatedDepartment = new DepartmentDTO
            {
                DepartmentName = "Computer Science",
                Building = "Building C"
            };

            // Act
            var result =
                await service.UpdateDepartmentAsync(1, updatedDepartment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Computer Science", result.DepartmentName);
            Assert.Equal("Building C", result.Building);
            Assert.Equal(1, result.DepartmentID);
        }


        // UPDATE - NOT FOUND
        [Fact]
        public async Task UpdateDepartmentAsync_ShouldThrowException_WhenDepartmentDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();

            var service = new DepartmentService(context);

            var updatedDepartment = new DepartmentDTO
            {
                DepartmentName = "Computer Science",
                Building = "Building C"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.UpdateDepartmentAsync(999, updatedDepartment));

            Assert.Equal("400 Bad Request", exception.Message);
        }


        // DELETE
        [Fact]
        public async Task DeleteDepartmentAsync_ShouldDeleteDepartment()
        {
            // Arrange
            using var context = CreateContext();

            context.Departments.Add(new Department
            {
                DepartmentID = 1,
                DepartmentName = "Information Technology",
                Building = "Building A"
            });

            await context.SaveChangesAsync();

            var service = new DepartmentService(context);

            // Act
            var result = await service.DeleteDepartmentAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.DepartmentID);

            var deletedDepartment =
                await context.Departments.FindAsync(1);

            Assert.Null(deletedDepartment);
        }


        // DELETE - NOT FOUND
        [Fact]
        public async Task DeleteDepartmentAsync_ShouldThrowException_WhenDepartmentDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();

            var service = new DepartmentService(context);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.DeleteDepartmentAsync(999));

            Assert.Equal("404 Not Found", exception.Message);
        }
    }
}
