using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Application.Interfaces;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class CustomerDoesNotConfirmedOrderDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<CustomerDoesNotConfirmedOrderDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(CustomerDoesNotConfirmedOrderDomainEvent domainEvent)
    {
        await orderRepository.ExecuteDeleteByUserIdAsync(domainEvent.Customer.Id);
    }
}