namespace University.MVC.Models
{
    public class StudentViewModel
    {
        public int StudentID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public int DepartmentID { get; set; }

        public DateTime EnrollmentYear { get; set; }

        public string Status { get; set; }
    }
}