using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentCpfDomainEvent(User user, CPF cpf) : IDomainEvent
{
    public User User { get; set; } = user;
    public CPF CPF { get; set; } = cpf;
    public string EventType { get; private set; } = "customer-sent-cpf";
}