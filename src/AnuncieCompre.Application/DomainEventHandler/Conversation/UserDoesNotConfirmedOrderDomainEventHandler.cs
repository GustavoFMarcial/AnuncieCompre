using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Application.Interfaces;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class UserDoesNotConfirmedOrderDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<UserDoesNotConfirmedOrderDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(UserDoesNotConfirmedOrderDomainEvent domainEvent)
    {
        await orderRepository.ExecuteDeleteByUserIdAsync(domainEvent.User.Id);
    }
}