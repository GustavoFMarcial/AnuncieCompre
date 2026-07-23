using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class CustomerSentCpfDomainEventHandler(IUserRepository _userRepository, ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerSentCpfDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerSentCpfDomainEvent domainEvent)
    {
        User? user = await userRepository.GetUserByPhoneAsync(domainEvent.Phone.Value);

        if (user is null) return;

        Customer customer = Customer.Create(user, domainEvent.Cpf);

        customerRepository.Add(customer);
        await unitOfWork.SaveChangesAsync();
    }
}