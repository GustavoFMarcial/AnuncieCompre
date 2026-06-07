using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Exceptions;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class VendorSentCnpjDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(User user, ValueObject data)
    {
        
        if (data is CNPJ cnpj)
        {
            return new VendorSentCnpjDomainEvent(user.Phone.Value, cnpj.Value);
        }

        throw new DomainException("Tipo inválido do ValueObject");
    }
}