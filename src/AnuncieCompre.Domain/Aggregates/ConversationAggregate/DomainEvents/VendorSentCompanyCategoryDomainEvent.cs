using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentCompanyCategoryDomainEvent(Phone phone, CompanyCategory companyCategory) : IDomainEvent
{
    public Phone Phone { get; set; } = phone;
    public CompanyCategory CompanyCategory { get; set; } = companyCategory;
    public string EventType { get; private set; } = "vendor-sent-company-category";
}