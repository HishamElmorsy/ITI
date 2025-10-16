using MVC_Lab_1.Models;

namespace MVC_Lab_1.Repo
{
    public class StudentMoc : IEntityRepo<Student>
    {
        List<Student> students = [new Student() { Id = 1, Name = "Yehia", Age = 22, Email = "Yehia@iti.gov" }];
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Student Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Student> GetAll()
        {
            return students;
        }

        public Student Insert(Student student)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public Student Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
