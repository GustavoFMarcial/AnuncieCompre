using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.UseCase.Interfaces;


namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserSentProductDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<UserSentProductDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(UserSentProductDomainEvent domainEvent)
    {
         Order? order = await orderRepository.GetLastOrderByUserId(domainEvent.User.Id);

        if (order is null) return;

        order.SetProduct(domainEvent.Product);
    }
}