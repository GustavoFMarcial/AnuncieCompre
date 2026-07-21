using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class CustomerDoesNotConfirmedOrderDomainEventHandler(IOrderRepository _orderRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerDoesNotConfirmedOrderDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerDoesNotConfirmedOrderDomainEvent domainEvent)
    {
        Order? order = await orderRepository.GetLastOrderByPhoneAsync(domainEvent.Phone.Value);

        if (order is null) return;

        orderRepository.Delete(order);
        await unitOfWork.SaveChangesAsync();
    }
}