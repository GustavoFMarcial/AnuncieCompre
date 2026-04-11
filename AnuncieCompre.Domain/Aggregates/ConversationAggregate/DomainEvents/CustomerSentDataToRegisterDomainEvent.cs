using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentDataToRegisterDomainEvent(Phone phone, Name name, Email email, UserType userType, CPF cpf) : IDomainEvent
{
    public Phone? Phone { get; set; } = phone;
    public Name? Name { get; set; } = name;
    public Email? Email { get; set; } = email;
    public UserType UserType { get; set; } = userType;
    public CPF CPF { get; set; } = cpf;
}