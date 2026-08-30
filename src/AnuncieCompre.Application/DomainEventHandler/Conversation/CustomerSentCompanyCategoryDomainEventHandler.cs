using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Application.Interfaces;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class CustomerSentCompanyCategoryDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<CustomerSentCompanyCategoryDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(CustomerSentCompanyCategoryDomainEvent domainEvent)
    {
        Order order = Order.Create(domainEvent.Customer, domainEvent.CompanyCategory);

        orderRepository.Add(order);
    }
}