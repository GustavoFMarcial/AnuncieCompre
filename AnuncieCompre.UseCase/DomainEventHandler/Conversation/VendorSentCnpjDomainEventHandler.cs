using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentCnpjDomainEventHandler(IDatabase _db) : BackgroundService
{
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:vendor-sent-cnpj", "workers", "vendor-sent-cnpj", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:vendor-sent-cnpj", "workers", "vendor-sent-cnpj", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];

                if (payload == null) continue;

                var domainEvent = JsonSerializer.Deserialize<VendorSentCnpjDomainEvent>(payload);

                if (domainEvent == null) continue;

                string key = $"session:{domainEvent.User.Phone.Value}";
                var json = JsonSerializer.Serialize(domainEvent.CNPJ);

                var hash = new HashEntry[]
                {
                    new("cnpj", json),
                };

                await db.HashSetAsync(key, hash);
                await db.StreamAcknowledgeAsync("events:vendor-sent-cnpj", "workers", message.Id);
            }
                
            await Task.Delay(1000, stoppingToken);
        }
    }
}