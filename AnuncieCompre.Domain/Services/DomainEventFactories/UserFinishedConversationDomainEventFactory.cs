using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class UserFinishedConversationDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(User user, ValueObject tempData)
    {
        return new UserFinishedConversationDomainEvent(user.Phone.Value);
    }
}