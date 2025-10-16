namespace MVC_Lab_1.Models.ViewModels
{
    public class GetEditDepartmentCourseVM
    {
        public Department Department { get; set; }
        public List<Course> CourseAlreadyExistsInDepartment { get; set; }
        public List<Course> CourseDoesntExistInDepartment { get; set; }

    }
}
