using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentDataToOrderDomainEventHandler(IOrderRepository _orderRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentDataToOrderDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentDataToOrderDomainEvent domainEvent)
    {
        Order order = Order.Create(domainEvent.UserPhone, domainEvent.Product, domainEvent.Quantity, domainEvent.Category);
        orderRepository.Add(order);

        await unitOfWork.SaveChangesAsync();
    }
}