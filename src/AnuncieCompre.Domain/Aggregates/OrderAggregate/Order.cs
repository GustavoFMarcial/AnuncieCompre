using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.OrderAggregate;

public class Order : BaseEntity
{
    public Guid UserId { get; private set; }
    public Customer Customer { get; private set; } = default!;
    public ValueObjects.CompanyCategory Category { get; private set; } = default!;
    public Product? Product { get; private set; } = default!;
    public Quantity? Quantity  { get; private set; } = default!;

    private Order(){}

    private Order(Customer customer, ValueObjects.CompanyCategory category)
    {
        UserId = customer.Id;
        Customer = customer;
        Category = category;
    }

    public static Order Create(Customer customer, ValueObjects.CompanyCategory category)
    {
        var order = new Order(customer, category);

        // var domainEvent = new OrderCreatedDomainEvent(userPhone.Value, product.Value, quantity.Value, category.Value.ToString());
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