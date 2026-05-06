using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentCpfDomainEventHandler(ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentCpfDomainEvent>
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentCpfDomainEvent domainEvent)
    {
        // User? user = await userRepository.GetByIdAsync(domainEvent.User.Id);

        // if (user is null) return;

        Customer customer = Customer.Create(domainEvent.User, domainEvent.CPF);
        
        customerRepository.Add(customer);
        await unitOfWork.SaveChangesAsync();
    }
}