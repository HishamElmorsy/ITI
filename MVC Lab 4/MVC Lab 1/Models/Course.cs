namespace MVC_Lab_1.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CrsName { get; set; }
        public int CrsDuration { get; set; }

        public List<Department> Departments { get; set; }
        public List<StudentCourse> CourseStudents { get; set; }
    }
}
