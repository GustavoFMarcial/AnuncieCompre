using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Application.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class UserSentEmailDomainEventHandler() : IDomainEventHandler<UserSentEmailDomainEvent>
{

    public async Task HandleAsync(UserSentEmailDomainEvent domainEvent)
    {
        domainEvent.User.SetEmail(domainEvent.Email);
    }
}