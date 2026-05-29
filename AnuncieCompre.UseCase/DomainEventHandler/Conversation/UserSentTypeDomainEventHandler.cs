using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentTypeDomainEventHandler(IDatabase _db) : BackgroundService
{
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:user-sent-type", "workers", "user-sent-type", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:user-sent-type", "workers", "user-sent-type", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];

                if (payload == null) return;

                var domainEvent = JsonSerializer.Deserialize<UserSentTypeDomainEvent>(payload);

                if (domainEvent == null) return;

                string key = $"user:{domainEvent.User.Phone.Value}";
                var json = JsonSerializer.Serialize(domainEvent.UserType);

                var hash = new HashEntry[]
                {
                    new("type", json),
                };

                await db.HashSetAsync(key, hash);
                await db.StreamAcknowledgeAsync("events:user-sent-type", "workers", message.Id);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}