using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class CustomerFinishedConversationDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(Customer customer, ValueObject tempData)
    {
        return new CustomerFinishedConversationDomainEvent(customer);
    }
}