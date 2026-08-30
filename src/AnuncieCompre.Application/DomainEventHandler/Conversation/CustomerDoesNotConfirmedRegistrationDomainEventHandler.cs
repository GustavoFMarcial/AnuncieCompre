using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Application.Interfaces;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class CustomerDoesNotConfirmedRegistrationDomainEventHandler(ICustomerRepository _customerRepository) : IDomainEventHandler<CustomerDoesNotConfirmedRegistrationDomainEvent>
{
    private readonly ICustomerRepository customerRepository = _customerRepository;

    public async Task HandleAsync(CustomerDoesNotConfirmedRegistrationDomainEvent domainEvent)
    {
        await customerRepository.ExecuteDeleteByCustomerIdAsync(domainEvent.Customer.Id);
    }
}