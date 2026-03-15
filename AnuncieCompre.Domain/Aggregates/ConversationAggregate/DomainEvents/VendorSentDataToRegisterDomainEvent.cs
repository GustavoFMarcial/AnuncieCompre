using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentDataToRegisterDomainEvent(User user, CompanyCategory category, VOCNPJ cnpj) : IDomainEvent
{
    public User User { get; set; } = user;
    public CompanyCategory CompanyCategory { get; set; } = category;
    public VOCNPJ CNPJ { get; set; } = cnpj;
}