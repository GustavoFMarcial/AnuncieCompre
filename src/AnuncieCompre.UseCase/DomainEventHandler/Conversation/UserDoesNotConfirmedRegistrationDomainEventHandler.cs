using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserDoesNotConfirmedRegistrationDomainEventHandler(IUserRepository _userRepository) : IDomainEventHandler<UserDoesNotConfirmedRegistrationDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;

    public async Task HandleAsync(UserDoesNotConfirmedRegistrationDomainEvent domainEvent)
    {
        await userRepository.ExecuteDeleteByUserIdAsync(domainEvent.User.Id);
    }
}