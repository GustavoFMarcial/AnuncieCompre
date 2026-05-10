using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentNameDomainEventHandler(IDatabase _db) : IDomainEventHandler<UserSentNameDomainEvent>
{
    private readonly IDatabase db = _db;

    public async Task HandleAsync(UserSentNameDomainEvent domainEvent)
    {
        var json = JsonSerializer.Serialize(domainEvent.Name);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("name", json),
        };

        await db.HashSetAsync(key, hash);
    }
}