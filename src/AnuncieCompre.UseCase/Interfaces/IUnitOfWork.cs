namespace AnuncieCompre.UseCase.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}