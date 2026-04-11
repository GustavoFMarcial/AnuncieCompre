using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class CustomerSentDataToRegisterDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(Phone userPhone, Dictionary<string, ValueObject> tempData)
    {
        if (tempData["Name"] is Name name && tempData["Email"] is Email email && tempData["UserType"] is UserType userType && tempData["CPF"] is CPF cpf)
        {
            return new CustomerSentDataToRegisterDomainEvent(userPhone, name, email, userType, cpf);
        }

        throw new NotImplementedException();
    }
}