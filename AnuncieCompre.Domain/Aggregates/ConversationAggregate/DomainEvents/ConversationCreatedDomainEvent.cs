using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class ConversationCreatedDomainEvent(Conversation conversation) : IDomainEvent
{
    public Conversation Conversation { get; set; } = conversation;
}