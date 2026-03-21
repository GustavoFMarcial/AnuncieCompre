using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentDataToRegisterDomainEvent(VOPhone phone, VOName name, VOEmail email, UserType userType, VOCPF cpf) : IDomainEvent
{
    public VOPhone? Phone { get; set; } = phone;
    // public int UserId { get; set; } = userId;
    public VOName? Name { get; set; } = name;
    public VOEmail? Email { get; set; } = email;
    public UserType UserType { get; set; } = userType;
    // public User User { get; set; } = user;
    public VOCPF CPF { get; set; } = cpf;
}