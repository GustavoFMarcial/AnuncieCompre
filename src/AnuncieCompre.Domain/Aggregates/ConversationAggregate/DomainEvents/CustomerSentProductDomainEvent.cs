using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentProductDomainEvent(string phone, string product) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string Product { get; set; } = product;
    public string EventType { get; private set; } = "customer-sent-product";
}