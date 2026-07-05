using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentCompanyNameDomainEvent(string phone, string name) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string Name { get; set; } = name;
    public string EventType { get; private set; } = "vendor-sent-comapany-name";
}