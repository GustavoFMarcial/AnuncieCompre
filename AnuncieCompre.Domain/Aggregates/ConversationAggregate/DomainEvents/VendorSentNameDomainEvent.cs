using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentNameDomainEvent(User user, Name name) : IDomainEvent
{
    public User User { get; set; } = user;
    public Name Name { get; set; } = name;
}