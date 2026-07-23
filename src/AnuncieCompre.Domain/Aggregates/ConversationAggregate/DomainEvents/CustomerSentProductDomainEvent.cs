using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentProductDomainEvent(Phone phone, Product product) : IDomainEvent
{
    public Phone Phone { get; set; } = phone;
    public Product Product { get; set; } = product;
    public string EventType { get; private set; } = "customer-sent-product";
}