using AnuncieCompre.Domain.Aggregates;

namespace AnuncieCompre.Domain.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(Guid id);
    public void Add(T entity);
    public void Delete(T entity);
}