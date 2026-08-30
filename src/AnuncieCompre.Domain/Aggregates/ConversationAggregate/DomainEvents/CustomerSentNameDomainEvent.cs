using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentNameDomainEvent(Customer customer, Name name) : IDomainEvent
{
    public Customer Customer { get; set; } = customer;
    public Name Name { get; set; } = name;
    public string EventType { get; private set; } = "user-sent-name";
}