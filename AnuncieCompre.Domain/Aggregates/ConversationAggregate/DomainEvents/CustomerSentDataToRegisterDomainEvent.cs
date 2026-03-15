using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentDataToRegisterDomainEvent(User user, VOCPF cpf) : IDomainEvent
{
    public User User { get; set; } = user;
    public VOCPF CPF { get; set; } = cpf;
}