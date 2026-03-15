using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentDataToRegisterDomainEventHandler(ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentDataToRegisterDomainEvent>
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentDataToRegisterDomainEvent domainEvent)
    {
        Customer client = Customer.Create(domainEvent.User, domainEvent.CPF);
        customerRepository.Add(client);

        await unitOfWork.SaveChangesAsync();
    }
}