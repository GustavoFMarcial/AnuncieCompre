using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentDataToRegisterDomainEvent(Phone phone, Name name, Email email, UserType userType) : IDomainEvent
{
    public Phone? Phone { get; set; } = phone;
    public Name? Name { get; set; } = name;
    public Email? Email { get; set; } = email;
    public UserType UserType { get; set; } = userType;
}