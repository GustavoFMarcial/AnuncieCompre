using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentQuantityDomainEventHandler(ICustomerRepository _customerRepository ,IOrderRepository _orderRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentQuantityDomainEvent>
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IOrderRepository orderRepository = _orderRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentQuantityDomainEvent domainEvent)
    {
        Customer? customer = await customerRepository.GetCustomerByPhoneAsync(domainEvent.User.Phone.Value);

        if (customer == null) return;

        Order? order = await orderRepository.GetLastOrderByCustomerId(customer.Id);

        if (order is null) return;

        order.SetQuantity(domainEvent.Quantity);
        await unitOfWork.SaveChangesAsync();
    }
}