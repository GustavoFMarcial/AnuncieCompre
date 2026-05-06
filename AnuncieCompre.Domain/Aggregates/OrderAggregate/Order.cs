using AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.OrderAggregate;

public class Order : BaseEntity
{
    // public Phone UserPhone { get; private set; } = default!;
    public int CustomerId { get; private set; }
    public Product? Product { get; private set; } = default!;
    public Quantity? Quantity  { get; private set; } = default!;
    public ValueObjects.CompanyCategory? Category { get; private set; } = default!;

    private Order(){}

    private Order(int customerId/*, Product product, Quantity quantity*/, ValueObjects.CompanyCategory category)
    {
        CustomerId = customerId;
        // Product = product;
        // Quantity = quantity;
        Category = category;
    }

    public static Order Create(int customerId, /*Product product, Quantity quantity,*/ ValueObjects.CompanyCategory category)
    {
        var order = new Order(customerId, /*userPhone, product, quantity*/ category);

        // var domainEvent = new OrderCreatedDomainEvent(userPhone, product, quantity, category);
        // order.AddDomainEvent(domainEvent);

        return order;
    }

    public Order SetCompanyCategory(ValueObjects.CompanyCategory companyCategory)
    {
        Category = companyCategory;
        return this;
    }

    public Order SetQuantity(Quantity quantity)
    {
        Quantity = quantity;
        return this;
    }

    public Order SetProduct(Product product)
    {
        Product = product;
        return this;
    }
}