using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Application.Interfaces;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class CustomerFinishedConversationDomainEventHandler(IConversationRepository _conversationRepository) : IDomainEventHandler<CustomerFinishedConversationDomainEvent>
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;

    public async Task HandleAsync(CustomerFinishedConversationDomainEvent domainEvent)
    {
        Domain.Aggregates.ConversationAggregate.Conversation? conversation = await conversationRepository.GetOpenConversationByUserIdAsync(domainEvent.Customer.Id);

        if (conversation is null) return;

        conversation.Close();
    }
}