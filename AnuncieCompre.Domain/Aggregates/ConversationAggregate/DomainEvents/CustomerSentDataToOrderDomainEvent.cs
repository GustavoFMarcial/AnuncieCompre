using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentDataToOrderDomainEvent : IDomainEvent
{
    public Phone UserPhone { get; set; } = default!;
    public Product Product { get; set; } = default!;
    public Quantity Quantity { get; set; } = default!;
    public ValueObjects.CompanyCategory Category { get; set; } = default!;

    public static CustomerSentDataToOrderDomainEvent Create(Phone phone, Product product, Quantity quantity, ValueObjects.CompanyCategory category)
    {
        return new CustomerSentDataToOrderDomainEvent
        {
            UserPhone = phone,
            Product = product,
            Quantity = quantity,
            Category = category,
        };
    }
}