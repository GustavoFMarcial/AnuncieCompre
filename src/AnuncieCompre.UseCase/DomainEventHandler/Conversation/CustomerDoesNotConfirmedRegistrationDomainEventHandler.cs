using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class CustomerDoesNotConfirmedRegistrationDomainEventHandler(IUserRepository _userRepository) : IDomainEventHandler<CustomerDoesNotConfirmedRegistrationDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;

    public async Task HandleAsync(CustomerDoesNotConfirmedRegistrationDomainEvent domainEvent)
    {
        await userRepository.ExecuteDeleteAsync(domainEvent.Phone.Value);
    }
}