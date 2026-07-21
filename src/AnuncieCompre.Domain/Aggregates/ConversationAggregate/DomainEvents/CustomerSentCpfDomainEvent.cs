using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentCpfDomainEvent(Phone phone, CPF cpf) : IDomainEvent
{
    public Phone Phone { get; set; } = phone;
    public CPF Cpf { get; set; } = cpf;
    public string EventType { get; private set; } = "customer-sent-cpf";
}