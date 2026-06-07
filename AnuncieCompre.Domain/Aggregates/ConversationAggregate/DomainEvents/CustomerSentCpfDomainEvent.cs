using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentCpfDomainEvent(string phone, string cpf) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string cpf { get; set; } = cpf;
    public string EventType { get; private set; } = "customer-sent-cpf";
}