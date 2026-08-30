using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentCompanyCategoryDomainEvent(Customer customer, CompanyCategory companyCategory) : IDomainEvent
{
    public Customer Customer { get; set; } = customer;
    public CompanyCategory CompanyCategory { get; set; } = companyCategory;
    public string EventType { get; private set; } = "customer-sent-company-category";
}