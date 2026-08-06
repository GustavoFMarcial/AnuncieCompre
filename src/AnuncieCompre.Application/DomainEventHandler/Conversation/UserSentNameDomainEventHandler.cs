using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Application.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class UserSentNameDomainEventHandler() : IDomainEventHandler<UserSentNameDomainEvent>
{

    public async Task HandleAsync(UserSentNameDomainEvent domainEvent)
    {
        domainEvent.User.SetName(domainEvent.Name);
    }
}