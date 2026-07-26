using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserSentEmailDomainEventHandler() : IDomainEventHandler<UserSentEmailDomainEvent>
{

    public async Task HandleAsync(UserSentEmailDomainEvent domainEvent)
    {
        domainEvent.User.SetEmail(domainEvent.Email);
    }
}