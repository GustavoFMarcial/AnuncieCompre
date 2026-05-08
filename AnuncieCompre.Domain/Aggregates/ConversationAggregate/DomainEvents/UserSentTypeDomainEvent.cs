using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentTypeDomainEvent(User user, UserType userType) : IDomainEvent
{
    public User User { get; set; } = user;
    public UserType UserType { get; set; } = userType;
}