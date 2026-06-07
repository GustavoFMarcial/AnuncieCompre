using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentNameDomainEvent(User user, Name name) : IDomainEvent
{
    public string Phone { get; set; } = user.Phone.Value;
    public string Name { get; set; } = name.Value;
    public string EventType { get; private set; } = "user-sent-name";
}