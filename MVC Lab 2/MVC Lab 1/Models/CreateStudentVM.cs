using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Lab_1.Models
{
    public class CreateStudentVM
    {
        public Student Student { get; set; }
        public List<Department> Departments { get; set; }
    }
}
