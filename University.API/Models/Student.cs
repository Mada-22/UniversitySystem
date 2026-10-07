
using System.ComponentModel.DataAnnotations;



namespace University.API.Models
{
    public class Student
    {
        //Controller input validation to check if the required properties are there
        [Required]
        public int StudentID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName {  get; set; }
        [Required]
        public string Email { get; set; }
        [Range(18, 30)]
        public int Age { get; set; }
        [Required]
        public string Gender {  get; set; }

        public int DepartmentID { get; set; }

        public DateTime EnrollmentYear {  get; set; }

        public string Status { get; set; }

     public Department Department { get; set; }

    }
}
