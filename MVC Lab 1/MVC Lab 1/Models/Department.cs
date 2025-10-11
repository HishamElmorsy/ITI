using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Lab_1.Models
{
    public class Department
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int Capacity { get; set; }
        public List<Student> Students { get; set; }
    }
}
