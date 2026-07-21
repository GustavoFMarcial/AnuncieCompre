using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class CustomerSentQuantityDomainEventHandler(IOrderRepository _orderRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentQuantityDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentQuantityDomainEvent domainEvent)
    {
        Order? order = await orderRepository.GetLastOrderByPhoneAsync(domainEvent.Phone.Value);

        if (order is null) return;

        order.SetQuantity(domainEvent.Quantity);
        await unitOfWork.SaveChangesAsync();
    }
}