using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentDataToOrderDomainEvent(VOPhone phone, VOProduct product, VOQuantity quantity, CompanyCategory category) : IDomainEvent
{
    public VOPhone UserPhone { get; set; } = phone;
    public VOProduct Product { get; set; } = product;
    public VOQuantity Quantity { get; set; } = quantity;
    public CompanyCategory Category { get; set; } = category;
}