using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Exceptions;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class UserSentTypeDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(User user, ValueObject data)
    {
        
        if (data is UserType userType)
        {
            return new UserSentTypeDomainEvent(user.Phone.Value, userType.Value.ToString());
        }

        throw new DomainException("Tipo inválido do ValueObject");
    }
}