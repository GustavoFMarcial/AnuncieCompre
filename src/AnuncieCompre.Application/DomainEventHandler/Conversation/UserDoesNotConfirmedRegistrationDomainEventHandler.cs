using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Application.Interfaces;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class UserDoesNotConfirmedRegistrationDomainEventHandler(IUserRepository _userRepository) : IDomainEventHandler<UserDoesNotConfirmedRegistrationDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;

    public async Task HandleAsync(UserDoesNotConfirmedRegistrationDomainEvent domainEvent)
    {
        await userRepository.ExecuteDeleteByUserIdAsync(domainEvent.User.Id);
    }
}