using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Application.Interfaces;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class CustomerSentQuantityDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<CustomerSentQuantityDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(CustomerSentQuantityDomainEvent domainEvent)
    {
        Order? order = await orderRepository.GetLastOrderByUserIdAsync(domainEvent.Customer.Id);

        if (order is null) return;

        order.SetQuantity(domainEvent.Quantity);
    }
}