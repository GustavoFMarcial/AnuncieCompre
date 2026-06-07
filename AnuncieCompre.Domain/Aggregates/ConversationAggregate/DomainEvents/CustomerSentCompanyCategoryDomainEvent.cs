using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentCompanyCategoryDomainEvent(string phone, string companyCategory) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string CompanyCategory { get; set; } = companyCategory;
    public string EventType { get; private set; } = "customer-sent-company-category";
}