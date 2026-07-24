using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserFinishedConversationDomainEvent(User user) : IDomainEvent
{
    public User User { get; set; } = user;
    public string EventType { get; private set; } = "user-finished-conversation";
}