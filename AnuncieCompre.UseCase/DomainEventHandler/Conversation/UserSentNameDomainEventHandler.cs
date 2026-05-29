using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentNameDomainEventHandler(IDatabase _db) : BackgroundService
{
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:user-sent-name", "workers", "user-sent-name", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:user-sent-name", "workers", "user-sent-name", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];

                if (payload == null) return;

                var domainEvent = JsonSerializer.Deserialize<UserSentNameDomainEvent>(payload);

                if (domainEvent == null) return;

                string key = $"user:{domainEvent.User.Phone.Value}";
                var json = JsonSerializer.Serialize(domainEvent.Name);

                var hash = new HashEntry[]
                {
                    new("name", json),
                };

                await db.HashSetAsync(key, hash);
                await db.StreamAcknowledgeAsync("events:user-sent-name", "workers", message.Id);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}