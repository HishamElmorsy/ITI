using Microsoft.EntityFrameworkCore;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;
using MVC_Lab_1.Models.ViewModels;

namespace MVC_Lab_1.Repo
{
    public class DepartmentCourseRepo
    {
        private ITIDbContext db;

        public DepartmentCourseRepo(ITIDbContext _db)
        {
            db = _db;
        }
        public GetEditDepartmentCourseVM GetDepartmentCourses(int DeptId)
        {
            GetEditDepartmentCourseVM model = new GetEditDepartmentCourseVM();
            model.Department = db.Departments.Include(d=>d.Courses).FirstOrDefault(s => s.DeptId == DeptId);
            model.CourseAlreadyExistsInDepartment = model.Department.Courses;
            var allCourses = db.Courses.ToList();
            model.CourseDoesntExistInDepartment = allCourses.Except(model.CourseAlreadyExistsInDepartment).ToList();
            return model;
        }
        public void UpdateDepartmentCourses(PostDepartmentCouseUpdateVM model)
        {
            var dept = db.Departments.Include(c => c.Courses).FirstOrDefault(c => c.DeptId == model.DeptId);
            if(model.CoursesToRemove != null)
            foreach(int i in model.CoursesToRemove)
            {
                var c = db.Courses.FirstOrDefault(s => s.Id == i);
                dept.Courses.Remove(c);
            }
            if (model.CoursesToAdd != null)

                foreach (int i in model.CoursesToAdd)
            {
                var c = db.Courses.FirstOrDefault(s => s.Id == i);
                dept.Courses.Add(c);
            }
            
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public Department GetDepartmentWithCourses(int id)
        {
            return db.Departments.Include(s => s.Courses).FirstOrDefault(s => s.DeptId == id);
        }
    }
}
