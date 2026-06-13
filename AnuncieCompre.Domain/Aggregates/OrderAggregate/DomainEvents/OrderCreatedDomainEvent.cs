using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;

public class OrderCreatedDomainEvent(string phone, string product, string quantity, string category) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string Product { get; set; } = product;
    public string Quantity { get; set; } = quantity;
    public string Category { get; set; } = category;
    public string EventType { get; private set; } = "order-created";
}