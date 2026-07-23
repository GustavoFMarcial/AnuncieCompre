using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentCnpjDomainEvent(Phone phone, CNPJ cnpj) : IDomainEvent
{
    public Phone Phone { get; set; } = phone;
    public CNPJ Cnpj { get; set; } = cnpj;
    public string EventType { get; private set; } = "vendor-sent-cnpj";
}