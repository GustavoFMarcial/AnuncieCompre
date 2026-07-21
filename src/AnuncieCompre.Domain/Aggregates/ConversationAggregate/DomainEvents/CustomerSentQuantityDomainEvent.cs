using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentQuantityDomainEvent(Phone phone, Quantity quantity) : IDomainEvent
{
    public Phone Phone { get; set; } = phone;
    public Quantity Quantity { get; set; } = quantity;
    public string EventType { get; private set; } = "customer-sent-quantity";
}