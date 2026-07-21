using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentNameDomainEvent(Phone phone, Name name) : IDomainEvent
{
    public Phone Phone { get; set; } = phone;
    public Name Name { get; set; } = name;
    public string EventType { get; private set; } = "user-sent-name";
}