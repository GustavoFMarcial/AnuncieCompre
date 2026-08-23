using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Application.Interfaces;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class UserSentQuantityDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<UserSentQuantityDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(UserSentQuantityDomainEvent domainEvent)
    {
        Order? order = await orderRepository.GetLastOrderByUserIdAsync(domainEvent.User.Id);

        if (order is null) return;

        order.SetQuantity(domainEvent.Quantity);
    }
}