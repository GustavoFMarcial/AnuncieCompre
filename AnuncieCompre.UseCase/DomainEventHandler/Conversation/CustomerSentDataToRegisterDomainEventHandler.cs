using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentDataToRegisterDomainEventHandler(IUserRepository _userRepository, ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentDataToRegisterDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentDataToRegisterDomainEvent domainEvent)
    {
        User? user = await userRepository.GetUserByPhoneAsync(domainEvent.Phone!.Value);
        
        Customer client = Customer.Create(user!, domainEvent.CPF);
        customerRepository.Add(client);

        await unitOfWork.SaveChangesAsync();
    }
}