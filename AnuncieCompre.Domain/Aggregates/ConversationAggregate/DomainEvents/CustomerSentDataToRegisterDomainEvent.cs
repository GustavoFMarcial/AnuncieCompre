using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentDataToRegisterDomainEvent(VOPhone phone, VOName name, VOEmail email, UserType userType, VOCPF cpf) : IDomainEvent
{
    public VOPhone? Phone { get; set; } = phone;
    public VOName? Name { get; set; } = name;
    public VOEmail? Email { get; set; } = email;
    public UserType UserType { get; set; } = userType;
    public VOCPF CPF { get; set; } = cpf;
}