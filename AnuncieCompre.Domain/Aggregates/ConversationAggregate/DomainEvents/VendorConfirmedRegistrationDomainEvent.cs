using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorConfirmedRegistrationDomainEvent(User user) : IDomainEvent
{
    public User User { get; set; } = user;
    public string EventType { get; private set; } = "vendor-confirmed-registration";
}