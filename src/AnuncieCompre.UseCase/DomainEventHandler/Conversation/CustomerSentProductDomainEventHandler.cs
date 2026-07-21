using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.UseCase.Interfaces;


namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class CustomerSentProductDomainEventHandler(IOrderRepository _orderRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentProductDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentProductDomainEvent domainEvent)
    {
        Order order = Order.Create(domainEvent.Phone, domainEvent.Product);

        orderRepository.Add(order);
        await unitOfWork.SaveChangesAsync();
    }
}