using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentProductDomainEvent(User user, Product product) : IDomainEvent
{
    public User User { get; set; } = user;
    public Product product { get; set; } = product;
    // public Name Name { get; set; } = name;
    // public Email Email { get; set; } = email;
    // // public UserType UserType { get; set; } = userType;
    // public CPF CPF { get; set; } = cpf;
}