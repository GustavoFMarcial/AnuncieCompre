namespace AnuncieCompre.Application.Interfaces;

public interface IApplicationTransaction : IAsyncDisposable
{
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    public Task RollbackAsync(CancellationToken cancellationToken = default);
}