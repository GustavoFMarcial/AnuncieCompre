using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerDoesNotConfirmedRegistrationDomainEvent(Customer customer) : IDomainEvent
{
    public Customer Customer { get; set; } = customer;
    public string EventType { get; private set; } = "customer-confirmed-registration";
}