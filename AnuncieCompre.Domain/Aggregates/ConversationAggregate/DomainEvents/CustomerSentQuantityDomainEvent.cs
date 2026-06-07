using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentQuantityDomainEvent(string phone, string quantity) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string Quantity { get; set; } = quantity;
    public string EventType { get; private set; } = "customer-sent-quantity";
}