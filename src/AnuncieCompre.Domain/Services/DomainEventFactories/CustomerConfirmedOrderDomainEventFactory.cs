using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class CustomerConfirmedOrderDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(User user, ValueObject tempData)
    {
        return new CustomerConfirmedOrderDomainEvent(user.Phone.Value);
    }
}