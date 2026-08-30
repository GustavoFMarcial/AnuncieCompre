using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentQuantityDomainEvent(Customer customer, Quantity quantity) : IDomainEvent
{
    public Customer Customer { get; set; } = customer;
    public Quantity Quantity { get; set; } = quantity;
    public string EventType { get; private set; } = "customer-sent-quantity";
}