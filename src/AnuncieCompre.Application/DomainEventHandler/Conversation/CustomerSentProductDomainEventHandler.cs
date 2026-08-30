using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Application.Interfaces;


namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class CustomerSentProductDomainEventHandler(IOrderRepository _orderRepository) : IDomainEventHandler<CustomerSentProductDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;

    public async Task HandleAsync(CustomerSentProductDomainEvent domainEvent)
    {
         Order? order = await orderRepository.GetLastOrderByUserIdAsync(domainEvent.Customer.Id);

        if (order is null) return;

        order.SetProduct(domainEvent.Product);
    }
}