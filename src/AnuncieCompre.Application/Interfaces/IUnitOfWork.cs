namespace AnuncieCompre.Application.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
    public Task<IApplicationTransaction> BeginTransactionAsync();
}