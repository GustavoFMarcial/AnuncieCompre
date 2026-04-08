using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;

public class OrderCreatedDomainEvent(Phone userPhone, Product product, Quantity quantity, ValueObjects.CompanyCategory category) : IDomainEvent
{
    public Phone UserPhone { get; set; } = userPhone;
    public Product Product { get; set; } = product;
    public Quantity Quantity { get; set; } = quantity;
    public ValueObjects.CompanyCategory Category { get; set; } = category;
}