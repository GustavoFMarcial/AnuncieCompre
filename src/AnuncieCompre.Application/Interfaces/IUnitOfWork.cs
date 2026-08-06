namespace AnuncieCompre.Application.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}