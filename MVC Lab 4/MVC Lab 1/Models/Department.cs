using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Lab_1.Models
{
    public class Department
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeptId { get; set; }
        [Required]
        public string DeptName { get; set; }
        [Range(1,50)]
        public int Capacity { get; set; }

        public bool Status { get; set; } = true;
        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
    }
}
