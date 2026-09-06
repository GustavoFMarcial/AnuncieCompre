using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates;

namespace AnuncieCompre.Application.Dispatchers;

public class EventDispatcher(IServiceProvider _serviceProvider)
{
    private readonly IServiceProvider serviceProvider = _serviceProvider;

    public async Task DispatchAsync(BaseEntity entity)
    {
        List<IDomainEvent> domainEvents = entity.DomainEvents.ToList();

        entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            Type eventType = domainEvent.GetType();
            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

            var handler = serviceProvider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod("HandleAsync");

            if (method != null)
            {
                var taskResult = method.Invoke(handler, [domainEvent]);

                if (taskResult is Task task)
                {
                    await task;
                }
            }
        }
    }
}