using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentEmailDomainEvent(Customer customer, Email email) : IDomainEvent
{
    public Customer Customer { get; set; } = customer;
    public Email Email { get; set; } = email;
    public string EventType { get; private set; } = "user-sent-email";
}