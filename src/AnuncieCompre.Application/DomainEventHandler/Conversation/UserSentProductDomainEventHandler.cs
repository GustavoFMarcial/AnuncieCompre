using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Application.Interfaces;


namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class UserSentProductDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<UserSentProductDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(UserSentProductDomainEvent domainEvent)
    {
         Order? order = await orderRepository.GetLastOrderByUserIdAsync(domainEvent.User.Id);

        if (order is null) return;

        order.SetProduct(domainEvent.Product);
    }
}