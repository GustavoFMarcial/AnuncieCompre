using AnuncieCompre.Infra.Data;
using AnuncieCompre.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using AnuncieCompre.Domain.Aggregates;

namespace AnuncieCompre.Infra.Repositories;

public class BaseRepository<T>(AnuncieCompreContext _context) : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DbContext context = _context;
    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
    }
}