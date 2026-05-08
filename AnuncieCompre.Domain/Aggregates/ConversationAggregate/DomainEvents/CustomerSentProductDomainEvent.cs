using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentProductDomainEvent(User user, Product product) : IDomainEvent
{
    public User User { get; set; } = user;
    public Product Product { get; set; } = product;
}