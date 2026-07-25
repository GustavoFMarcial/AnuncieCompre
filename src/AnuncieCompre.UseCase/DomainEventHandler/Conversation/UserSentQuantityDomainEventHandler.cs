using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserSentQuantityDomainEventHandler(IOrderRepository _orderRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<UserSentQuantityDomainEvent>
{
    private readonly IOrderRepository orderRepository = _orderRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserSentQuantityDomainEvent domainEvent)
    {
        Order? order = await orderRepository.GetLastOrderByUserId(domainEvent.User.Id);

        if (order is null) return;

        order.SetQuantity(domainEvent.Quantity);
        await unitOfWork.SaveChangesAsync();
    }
}