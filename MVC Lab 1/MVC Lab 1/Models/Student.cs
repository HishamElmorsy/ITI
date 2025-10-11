using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Lab_1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        
        [ForeignKey("Department")]
        public int DeptNo { get; set; }
        public Department Department { get; set; }
    }
}
