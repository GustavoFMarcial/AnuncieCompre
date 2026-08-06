using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    public Task HandleAsync(T domainEvent);
}