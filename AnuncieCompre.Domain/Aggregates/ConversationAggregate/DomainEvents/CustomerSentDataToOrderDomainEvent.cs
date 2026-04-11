using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentDataToOrderDomainEvent(Phone phone, Product product, Quantity quantity, CompanyCategory category) : IDomainEvent
{
    public Phone UserPhone { get; set; } = phone!;
    public Product Product { get; set; } = product!;
    public Quantity Quantity { get; set; } = quantity!;
    public CompanyCategory Category { get; set; } = category!;
}