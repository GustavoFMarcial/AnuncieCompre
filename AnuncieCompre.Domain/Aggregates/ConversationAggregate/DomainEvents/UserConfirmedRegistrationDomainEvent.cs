using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserConfirmedRegistrationDomainEvent(User user) : IDomainEvent
{
    public User User { get; set; } = user;
}