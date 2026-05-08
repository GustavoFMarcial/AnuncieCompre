using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentCnpjDomainEvent(User user, CNPJ cnpj) : IDomainEvent
{
    public User User { get; set; } = user;
    public CNPJ CNPJ { get; set; } = cnpj;
}