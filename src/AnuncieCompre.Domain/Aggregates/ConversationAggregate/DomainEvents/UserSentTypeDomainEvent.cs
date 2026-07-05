using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentTypeDomainEvent(string phone, string userType) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string UserType { get; set; } = userType;
    public string EventType { get; private set; } = "user-sent-type";
}