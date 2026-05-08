using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentEmailDomainEvent(User user, Email email) : IDomainEvent
{
    public User User { get; set; } = user;
    public Email Email { get; set; } = email;
}