using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerSentCompanyCategoryDomainEvent(User user, CompanyCategory companyCategory) : IDomainEvent
{
    public User User { get; set; } = user;
    public CompanyCategory CompanyCategory { get; set; } = companyCategory;
    // public Name Name { get; set; } = name;
    // public Email Email { get; set; } = email;
    // // public UserType UserType { get; set; } = userType;
    // public CPF CPF { get; set; } = cpf;
}