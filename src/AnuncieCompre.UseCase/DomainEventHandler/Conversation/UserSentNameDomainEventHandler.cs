using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserSentNameDomainEventHandler() : IDomainEventHandler<UserSentNameDomainEvent>
{

    public async Task HandleAsync(UserSentNameDomainEvent domainEvent)
    {
        domainEvent.User.SetName(domainEvent.Name);
    }
}