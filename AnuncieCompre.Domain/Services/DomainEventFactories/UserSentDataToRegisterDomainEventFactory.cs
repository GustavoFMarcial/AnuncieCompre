using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class UserSentDataToRegisterDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(Phone userPhone, Dictionary<string, ValueObject> tempData)
    {
        if (tempData["Name"] is Name name && tempData["Email"] is Email email && tempData["UserType"] is UserType userType)
        {
            return new UserSentDataToRegisterDomainEvent(userPhone, name, email, userType);
        }

        throw new NotImplementedException();
    }
}