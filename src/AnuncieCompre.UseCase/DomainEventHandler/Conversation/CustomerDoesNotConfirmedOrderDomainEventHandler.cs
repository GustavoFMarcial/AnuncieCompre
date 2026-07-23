using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class CustomerDoesNotConfirmedOrderDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<CustomerDoesNotConfirmedOrderDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(CustomerDoesNotConfirmedOrderDomainEvent domainEvent)
    {
        await orderRepository.ExecuteDeleteAsync(domainEvent.Phone.Value);
    }
}