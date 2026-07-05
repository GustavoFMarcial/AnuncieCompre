using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.UseCase.Interfaces;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    public Task HandleAsync(T domainEvent);
}