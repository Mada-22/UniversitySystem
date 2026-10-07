using System.ComponentModel.DataAnnotations;

namespace University.API.Models
{
    public class Department
    {
        [Required]
        public int DepartmentID { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        [Required]
        public string Building { get; set; }
        public decimal? Budget { get; set; }
        public string? Status { get; set; }

        public List<Student> Students { get; set; }

       
    }
}
