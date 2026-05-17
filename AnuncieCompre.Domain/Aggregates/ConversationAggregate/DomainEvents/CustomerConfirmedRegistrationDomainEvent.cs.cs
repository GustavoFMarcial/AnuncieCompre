using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerConfirmedRegistrationDomainEvent(User user) : IDomainEvent
{
    public User User { get; set; } = user;
    public string EventType { get; private set; } = "customer-confirmed-registration";
}