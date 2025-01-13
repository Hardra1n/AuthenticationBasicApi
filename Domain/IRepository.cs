using System.Dynamic;

namespace Domain;

public interface IRepository<T>
{
    public IEnumerable<T> GetAll();
    public T GetById(Guid id);
    public void Delete(Guid id);
    public void Update(T entity);
    public void Add(T entity);
}