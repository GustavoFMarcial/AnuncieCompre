using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserFinishedConversationDomainEventHandler(IConversationRepository _conversationRepository) : IDomainEventHandler<UserFinishedConversationDomainEvent>
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;

    public async Task HandleAsync(UserFinishedConversationDomainEvent domainEvent)
    {
        Domain.Aggregates.ConversationAggregate.Conversation? conversation = await conversationRepository.GetOpenConversationByUserIdAsync(domainEvent.User.Id);

        if (conversation is null) return;

        conversation.Close();
    }
}