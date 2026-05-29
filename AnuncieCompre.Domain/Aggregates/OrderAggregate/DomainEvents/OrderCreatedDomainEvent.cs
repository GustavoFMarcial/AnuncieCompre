using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;

public class OrderCreatedDomainEvent(int customerId, Product product, Quantity quantity, ValueObjects.CompanyCategory category) : IDomainEvent
{
    public int CustomerId { get; set; } = customerId;
    public Product Product { get; set; } = product;
    public Quantity Quantity { get; set; } = quantity;
    public ValueObjects.CompanyCategory Category { get; set; } = category;
    public string EventType { get; private set; } = "order-created";
}