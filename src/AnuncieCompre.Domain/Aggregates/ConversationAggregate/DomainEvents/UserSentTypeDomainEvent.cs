using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentTypeDomainEvent(Phone phone, UserType userType) : IDomainEvent
{
    public Phone Phone { get; set; } = phone;
    public UserType UserType { get; set; } = userType;
    public string EventType { get; private set; } = "user-sent-type";
}