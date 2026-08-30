using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentProductDomainEvent(Customer customer, Product product) : IDomainEvent
{
    public Customer Customer { get; set; } = customer;
    public Product Product { get; set; } = product;
    public string EventType { get; private set; } = "customer-sent-product";
}