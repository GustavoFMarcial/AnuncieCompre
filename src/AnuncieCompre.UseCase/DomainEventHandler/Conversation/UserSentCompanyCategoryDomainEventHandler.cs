using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserSentCompanyCategoryDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<UserSentCompanyCategoryDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(UserSentCompanyCategoryDomainEvent domainEvent)
    {
        Order order = Order.Create(domainEvent.User, domainEvent.CompanyCategory);

        orderRepository.Add(order);
    }
}