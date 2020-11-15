namespace Infrastructure
{
    public interface IDataBase<T>
    {
        void Add(T obj);
        void Remove(T obj);

        void Select(string key);

        void Insert(T obj);
    }
}