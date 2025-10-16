using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Lab_1.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =3)]
        public string Name { get; set; }
        [Range(15,30)]
        public int Age { get; set; }
        [Required,StringLength(50,MinimumLength =3)]

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [ForeignKey("Department")]
        public int DeptNo { get; set; }
        public Department Department { get; set; }
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
