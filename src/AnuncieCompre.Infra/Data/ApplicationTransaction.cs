using AnuncieCompre.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnuncieCompre.Infra.Data;

public class ApplicationTransaction(IDbContextTransaction _transaction) : IApplicationTransaction
{
    private readonly IDbContextTransaction transaction = _transaction;

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        return transaction.CommitAsync(cancellationToken);
    } 

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return transaction.RollbackAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        return transaction.DisposeAsync();
    }
}