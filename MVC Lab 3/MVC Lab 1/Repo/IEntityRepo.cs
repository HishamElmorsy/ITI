namespace MVC_Lab_1.Repo
{
    public interface IEntityRepo<T>
    {
        List<T> GetAll();
        T Get(int id);
        T Insert(T student);
        T Update(T student);

        void Delete(int id);
        int Save();

    }
}
