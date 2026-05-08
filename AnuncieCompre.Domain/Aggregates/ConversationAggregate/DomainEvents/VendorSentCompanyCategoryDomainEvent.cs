using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentCompanyCategoryDomainEvent(User user, CompanyCategory companyCategory) : IDomainEvent
{
    public User User { get; set; } = user;
    public CompanyCategory CompanyCategory { get; set; } = companyCategory;
}