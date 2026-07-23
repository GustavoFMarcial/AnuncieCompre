using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerDoesNotConfirmedRegistrationDomainEvent(Phone phone) : IDomainEvent
{
    public Phone Phone { get; set; } = phone;
    public string EventType { get; private set; } = "customer-confirmed-registration";
}