using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentNameDomainEvent(User user, Name name) : IDomainEvent
{
    public User User { get; set; } = user;
    public Name Name { get; set; } = name;
    // public Email? Email { get; set; } = email;
    // // public UserType UserType { get; set; } = userType;
    // public CPF CPF { get; set; } = cpf;
}