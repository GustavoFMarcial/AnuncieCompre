using AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.OrderAggregate;

public class Order : BaseEntity
{
    public Phone UserPhone { get; private set; } = default!;
    public Product Product { get; private set; } = default!;
    public Quantity Quantity  { get; private set; } = default!;
    public CompanyCategory Category { get; private set; }

    private Order(){}

    private Order(Phone userPhone, Product product, Quantity quantity, CompanyCategory category)
    {
        UserPhone = userPhone;
        Product = product;
        Quantity = quantity;
        Category = category;
    }

    public static Order Create(Phone userPhone, Product product, Quantity quantity, CompanyCategory category)
    {
        var order = new Order(userPhone, product, quantity, category);

        var domainEvent = new OrderCreatedDomainEvent(userPhone, product, quantity, category);
        order.AddDomainEvent(domainEvent);

        return order;
    }

    public Order SetProduct(Product product)
    {
        Product = product;
        return this;
    }

    public Order SetQuantity(Quantity quantity)
    {
        Quantity = quantity;
        return this;
    }
}