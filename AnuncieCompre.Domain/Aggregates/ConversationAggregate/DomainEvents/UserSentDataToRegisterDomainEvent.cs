using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class UserSentDataToRegisterDomainEvent(int userId, VOName name, VOEmail email, UserType userType) : IDomainEvent
{
    public int UserId { get; set; } = userId;
    public VOName? Name { get; set; } = name;
    public VOEmail? Email { get; set; } = email;
    public UserType UserType { get; set; } = userType;
}