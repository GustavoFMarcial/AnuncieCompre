using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.UseCase.Dispatcher;
public interface IDomainEventDispatcher
{
    public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}