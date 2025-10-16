namespace MVC_Lab_1.Models.ViewModels
{
    public class PostDepartmentCouseUpdateVM
    {
        public int DeptId { get; set; }
        public int[] CoursesToRemove { get; set; }
        public int[] CoursesToAdd { get; set; }

    }
}
