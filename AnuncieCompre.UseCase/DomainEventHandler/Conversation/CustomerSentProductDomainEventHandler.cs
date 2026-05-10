using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentProductDomainEventHandler(ICustomerRepository _customerRepository,IOrderRepository _orderRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentProductDomainEvent>
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IOrderRepository orderRepository = _orderRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentProductDomainEvent domainEvent)
    {
        Customer? customer = await customerRepository.GetCustomerByPhoneAsync(domainEvent.User.Phone.Value);

        if (customer == null) return;

        Order? order = await orderRepository.GetLastOrderByCustomerId(customer.Id);

        if (order is null) return;

        order.SetProduct(domainEvent.Product);
        await unitOfWork.SaveChangesAsync();
    }
}