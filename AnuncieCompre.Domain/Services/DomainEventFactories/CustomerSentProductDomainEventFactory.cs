using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Exceptions;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class CustomerSentProductDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(User user, ValueObject data)
    {
        
        if (data is Product product)
        {
            return new CustomerSentProductDomainEvent(user.Phone.Value, product.Value);
        }

        throw new DomainException("Tipo inválido do ValueObject");
    }
}