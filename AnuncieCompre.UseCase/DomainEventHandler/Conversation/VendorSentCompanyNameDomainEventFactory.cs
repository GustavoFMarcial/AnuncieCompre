using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentCompanyNameDomainEventHandler(IDatabase _db) : BackgroundService
{
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:vendor-sent-comapany-name", "workers", "vendor-sent-comapany-name", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:vendor-sent-comapany-name", "workers", "vendor-sent-comapany-name", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];

                if (payload == null) continue;

                var domainEvent = JsonSerializer.Deserialize<VendorSentCompanyNameDomainEvent>(payload);

                if (domainEvent == null) continue;

                string key = $"session:{domainEvent.Phone}";

                var hash = new HashEntry[]
                {
                    new("companyName", domainEvent.Name),
                };

                await db.HashSetAsync(key, hash);
                await db.StreamAcknowledgeAsync("events:vendor-sent-comapany-name", "workers", message.Id);
            }
                
            await Task.Delay(1000, stoppingToken);
        }
    }
}