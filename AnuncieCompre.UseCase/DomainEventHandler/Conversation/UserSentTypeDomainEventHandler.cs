using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentTypeDomainEventHandler(IDatabase _db) : IDomainEventHandler<UserSentTypeDomainEvent>
{
    private readonly IDatabase db = _db;

    public async Task HandleAsync(UserSentTypeDomainEvent domainEvent)
    {
        var json = JsonSerializer.Serialize(domainEvent.UserType);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("type", json),
        };

        await db.HashSetAsync(key, hash);
    }
}