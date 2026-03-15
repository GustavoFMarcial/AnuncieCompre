using AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.OrderAggregate;

public class Order : BaseEntity
{
    public VOPhone UserPhone { get; private set; } = default!;
    public VOProduct Product { get; private set; } = default!;
    public VOQuantity Quantity  { get; private set; } = default!;
    public CompanyCategory Category { get; private set; }

    private Order(){}

    private Order(VOPhone userPhone, VOProduct product, VOQuantity quantity, CompanyCategory category)
    {
        UserPhone = userPhone;
        Product = product;
        Quantity = quantity;
        Category = category;
    }

    public static Order Create(VOPhone userPhone, VOProduct product, VOQuantity quantity, CompanyCategory category)
    {
        var order = new Order(userPhone, product, quantity, category);

        var domainEvent = new OrderCreatedDomainEvent(userPhone, product, quantity, category);
        order.AddDomainEvent(domainEvent);

        return order;
    }

    public Order SetProduct(VOProduct product)
    {
        Product = product;
        return this;
    }

    public Order SetQuantity(VOQuantity quantity)
    {
        Quantity = quantity;
        return this;
    }
}