using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentQuantityDomainEvent(User user, Quantity quantity) : IDomainEvent
{
    public User User { get; set; } = user;
    public Quantity Quantity { get; set; } = quantity;
    public string EventType { get; private set; } = "customer-sent-quantity";
}