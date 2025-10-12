using Microsoft.EntityFrameworkCore;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;

namespace MVC_Lab_1.Repo
{
    public interface IStudentEmailExist
    {
        public bool IsExist(string Email);
    }
    

    public class StudentRepo : IEntityRepo<Student>,IStudentEmailExist
    {
        static ITIDbContext dbContext = new ITIDbContext();

        public bool IsExist(string email)
        {
            return dbContext.Students.Any(s => s.Email == email);
        }
        public void Delete(int id)
        {
            var s = dbContext.Students.FirstOrDefault(x => x.Id == id);
            if (s != null)
            {
                dbContext.Students.Remove(s);
            }
        }


        public Student Get(int id)
        {
            return dbContext.Students
                .Include(s => s.Department)
                .FirstOrDefault(s => s.Id == id);
        }


        public List<Student> GetAll()
        {
            return dbContext.Students.Include(s=>s.Department).ToList();
        }

        public Student Insert(Student student)
        {
             dbContext.Students.Add(student);
            return student;
        }

        public int Save()
        {
            return dbContext.SaveChanges();
        }

        public Student Update(Student student)
        {
            dbContext.ChangeTracker.Clear();
            dbContext.Update(student);
            return student;
        }
    }
}
