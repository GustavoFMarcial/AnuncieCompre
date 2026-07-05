using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentEmailDomainEvent(string phone, string email) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string Email { get; set; } = email;
    public string EventType { get; private set; } = "user-sent-email";
}