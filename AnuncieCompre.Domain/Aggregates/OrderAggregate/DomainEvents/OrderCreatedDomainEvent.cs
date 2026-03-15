using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;

public class OrderCreatedDomainEvent(VOPhone userPhone, VOProduct product, VOQuantity quantity, CompanyCategory category) : IDomainEvent
{
    public VOPhone UserPhone { get; set; } = userPhone;
    public VOProduct Product { get; set; } = product;
    public VOQuantity Quantity { get; set; } = quantity;
    public CompanyCategory Category { get; set; } = category;
}