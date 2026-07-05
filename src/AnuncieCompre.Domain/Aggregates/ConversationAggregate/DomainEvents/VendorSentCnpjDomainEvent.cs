using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentCnpjDomainEvent(string phone, string cnpj) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string Cnpj { get; set; } = cnpj;
    public string EventType { get; private set; } = "vendor-sent-cnpj";
}