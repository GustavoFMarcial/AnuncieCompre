using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class VendorSentDataToRegisterDomainEvent(Phone phone, Name name, Email email, UserType userType, CompanyCategory category, CNPJ cnpj) : IDomainEvent
{
    public Phone? Phone { get; set; } = phone;
    // public int UserId { get; set; } = userId;
    public Name? Name { get; set; } = name;
    public Email? Email { get; set; } = email;
    public UserType UserType { get; set; } = userType;
    // public User User { get; set; } = user;
    public CompanyCategory CompanyCategory { get; set; } = category;
    public CNPJ CNPJ { get; set; } = cnpj;
}