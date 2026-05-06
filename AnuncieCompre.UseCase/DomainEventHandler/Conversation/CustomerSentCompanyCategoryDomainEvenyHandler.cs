using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentCompanyCategoryDomainEventHandler(ICustomerRepository _customerRepository ,IOrderRepository _orderRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentCompanyCategoryDomainEvent>
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IOrderRepository orderRepository = _orderRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentCompanyCategoryDomainEvent domainEvent)
    {
        Customer? customer = await customerRepository.GetCustomerByPhoneAsync(domainEvent.User.Phone.Value);

        if (customer == null) return;

        Order order = Order.Create(customer.Id, domainEvent.CompanyCategory);
        
        orderRepository.Add(order);
        await unitOfWork.SaveChangesAsync();
    }
}