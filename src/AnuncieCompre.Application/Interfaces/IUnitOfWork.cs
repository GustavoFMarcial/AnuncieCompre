using Microsoft.EntityFrameworkCore.Storage;

namespace AnuncieCompre.Application.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
    public Task<IDbContextTransaction> BeginTransactionAsync();
    public Task CommitTransactionAsync();
}