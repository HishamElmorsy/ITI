using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;

namespace MVC_Lab_1.Repo
{

    public class DepartmentRepo:IEntityRepo<Department>
    {
        ITIDbContext dbContext = new ITIDbContext();
        public void Delete(int id)
        {
            var d = dbContext.Departments.Include(s=>s.Students).FirstOrDefault(a=>a.DeptId==id);
            if (d.Students.Count == 0)
                dbContext.Departments.Remove(d);
            else
            {
                d.Status = false;
                Update(d);
            }

        }

        public Department Get(int id)
        {
            return dbContext.Departments.Find(id);
        }

        public List<Department> GetAll()
        {
            return dbContext.Departments.Where(s=>s.Status==true).ToList();
        }

        public Department Insert(Department department)
        {
            dbContext.Departments.Add(department);
            return department;
        }

        public int Save()
        {
            return dbContext.SaveChanges();
        }

        public Department Update(Department department)
        {
            dbContext.Update(department);
            return department;
        }
    }
}
