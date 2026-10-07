using Microsoft.EntityFrameworkCore;
using University.API.Data;
using University.API.DTOs;
using University.API.Models;
using University.API.Service;

namespace University.Test
{
    public class StudentsServiceTest
    {
        private UniversityDBContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<UniversityDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new UniversityDBContext(options);
        }

        // GET ALL STUDENTS
        [Fact]
        public async Task GetAllStudentsAsync_ShouldReturnAllStudents()
        {
            // Arrange
            using var context = CreateContext();

            context.Students.AddRange(
                new Student
                {
                    StudentID = 1,
                    FirstName = "Matthew",
                    LastName = "Davids",
                    Email = "matthew@test.com",
                    Age = 18,
                    Gender = "M",
                    DepartmentID = 1,
                    EnrollmentYear = DateTime.Now,
                    Status = "Acitve"
                },
                new Student
                {
                    StudentID = 2,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john@test.com",
                    Age = 20,
                    Gender = "M",
                    DepartmentID = 1,
                    EnrollmentYear = DateTime.Now,
                    Status = "Acitve"
                }
            );

            await context.SaveChangesAsync();

            var service = new StudentService(context);

            // Act
            var result = await service.GetAllStudentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        // GET STUDENT BY ID - SUCCESS
        [Fact]
        public async Task GetStudentAsync_ShouldReturnStudent()
        {
            // Arrange
            using var context = CreateContext();

            context.Students.Add(new Student
            {
                StudentID = 1,
                FirstName = "Matthew",
                LastName = "Davids",
                Email = "matthew@test.com",
                Age = 18,
                Gender = "M",
                DepartmentID = 1,
                EnrollmentYear = DateTime.Now,
                Status = "Acitve"
            });

            await context.SaveChangesAsync();

            var service = new StudentService(context);

            // Act
            var result = await service.GetStudentAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.StudentID);
            Assert.Equal("Matthew", result.FirstName);
            Assert.Equal("Davids", result.LastName);
            Assert.Equal("matthew@test.com", result.Email);
            Assert.Equal(18, result.Age);
            Assert.Equal("M", result.Gender);
        }

        // GET STUDENT BY ID - NOT FOUND
        [Fact]
        public async Task GetStudentAsync_ShouldThrowException_WhenStudentDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();

            var service = new StudentService(context);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.GetStudentAsync(999)
            );

            Assert.Equal("404 Not Found", exception.Message);
        }

        // CREATE STUDENT - SUCCESS
        [Fact]
        public async Task CreateStudentAsync_ShouldCreateStudent()
        {
            // Arrange
            using var context = CreateContext();

            var service = new StudentService(context);

            var studentDTO = new StudentDTO
            {
                FirstName = "Matthew",
                LastName = "Davids",
                Email = "matthew@test.com",
                Age = 18,
                Gender = "M",
                DepartmentID = 1,
                EnrollmentYear = DateTime.Now,
                Status = "Acitve"
            };

            // Act
            var result = await service.CreateStudentAsync(studentDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Matthew", result.FirstName);
            Assert.Equal("Davids", result.LastName);
            Assert.Equal("matthew@test.com", result.Email);
            Assert.Equal(18, result.Age);
            Assert.Equal("M", result.Gender);
            Assert.Equal(1, result.DepartmentID);
            Assert.Equal("Acitve", result.Status);
        }

        // CREATE STUDENT - NULL
        [Fact]
        public async Task CreateStudentAsync_ShouldThrowException_WhenStudentIsNull()
        {
            // Arrange
            using var context = CreateContext();

            var service = new StudentService(context);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.CreateStudentAsync(null)
            );

            Assert.Equal(
                "Student details are required.",
                exception.Message
            );
        }

        // CREATE STUDENT - DUPLICATE EMAIL
        [Fact]
        public async Task CreateStudentAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            using var context = CreateContext();

            context.Students.Add(new Student
            {
                StudentID = 1,
                FirstName = "Matthew",
                LastName = "Davids",
                Email = "matthew@test.com",
                Age = 18,
                Gender = "M",
                DepartmentID = 1,
                EnrollmentYear = DateTime.Now,
                Status = "Acitve"
            });

            await context.SaveChangesAsync();

            var service = new StudentService(context);

            var studentDTO = new StudentDTO
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "matthew@test.com",
                Age = 20,
                Gender = "M",
                DepartmentID = 1,
                EnrollmentYear = DateTime.Now,
                Status = "Acitve"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.CreateStudentAsync(studentDTO)
            );

            Assert.Equal("409 Conflict", exception.Message);
        }

        // UPDATE STUDENT - SUCCESS
        [Fact]
        public async Task UpdateStudentAsync_ShouldUpdateStudent()
        {
            // Arrange
            using var context = CreateContext();

            context.Students.Add(new Student
            {
                StudentID = 1,
                FirstName = "Matthew",
                LastName = "Davids",
                Email = "old@test.com",
                Age = 18,
                Gender = "M",
                DepartmentID = 1,
                EnrollmentYear = DateTime.Now,
                Status = "Acitve"
            });

            await context.SaveChangesAsync();

            var service = new StudentService(context);

            var updatedStudent = new StudentDTO
            {
                FirstName = "Matthew",
                LastName = "Davids",
                Email = "new@test.com",
                Age = 19,
                Gender = "M",
                DepartmentID = 1,
                EnrollmentYear = DateTime.Now,
                Status = "Acitve"
            };

            // Act
            var result = await service.UpdateStudentAsync(
                1,
                updatedStudent
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.StudentID);
            Assert.Equal("Matthew", result.FirstName);
            Assert.Equal("Davids", result.LastName);
            Assert.Equal("new@test.com", result.Email);
            Assert.Equal(19, result.Age);
        }

        // UPDATE STUDENT - NOT FOUND
        [Fact]
        public async Task UpdateStudentAsync_ShouldThrowException_WhenStudentDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();

            var service = new StudentService(context);

            var updatedStudent = new StudentDTO
            {
                FirstName = "Matthew",
                LastName = "Davids",
                Email = "matthew@test.com",
                Age = 19,
                Gender = "M",
                DepartmentID = 1,
                EnrollmentYear = DateTime.Now,
                Status = "Acitve"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.UpdateStudentAsync(
                    999,
                    updatedStudent
                )
            );

            Assert.Equal("404 Not Found", exception.Message);
        }

        // DELETE STUDENT - SUCCESS
        [Fact]
        public async Task DeleteStudentByIDAsync_ShouldDeleteStudent()
        {
            // Arrange
            using var context = CreateContext();

            context.Students.Add(new Student
            {
                StudentID = 1,
                FirstName = "Matthew",
                LastName = "Davids",
                Email = "matthew@test.com",
                Age = 18,
                Gender = "M",
                DepartmentID = 1,
                EnrollmentYear = DateTime.Now,
                Status = "Acitve"
            });

            await context.SaveChangesAsync();

            var service = new StudentService(context);

            // Act
            var result = await service.DeleteStudentByIDAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.StudentID);

            var deletedStudent =
                await context.Students.FindAsync(1);

            Assert.Null(deletedStudent);
        }

        // DELETE STUDENT - NOT FOUND
        [Fact]
        public async Task DeleteStudentByIDAsync_ShouldThrowException_WhenStudentDoesNotExist()
        {
            // Arrange
            using var context = CreateContext();

            var service = new StudentService(context);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.DeleteStudentByIDAsync(999)
            );

            Assert.Equal("404 Not Found", exception.Message);
        }
    }
}