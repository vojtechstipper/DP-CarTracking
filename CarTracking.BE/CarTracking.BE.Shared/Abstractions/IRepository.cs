namespace CarTracking.BE.Shared.Abstractions;

public interface IRepository<T>
{
    Task<T?> GetById(string id);
    Task<IEnumerable<T>> GetAll();
    Task SaveAsync();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}