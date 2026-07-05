using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using StackExchange.Redis;


namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentProductDomainEventHandler(IDatabase _db) : BackgroundService
{
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:customer-sent-product", "workers", "customer-sent-product", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:customer-sent-product", "workers", "customer-sent-product", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];
                var eventKey = $"processed-events:{eventId}";

                if (payload == null || eventId == null) continue;

                if (await db.KeyExistsAsync(eventKey))
                {
                    await db.StreamAcknowledgeAsync("events:customer-confirmed-registration", "workers", message.Id);
                    continue;
                }

                var domainEvent = JsonSerializer.Deserialize<CustomerSentProductDomainEvent>(payload);

                if (domainEvent == null) continue;

                string sessionKey = $"session:{domainEvent.Phone}";

                var hash = new HashEntry[]
                {
                    new("product", domainEvent.Product),
                };

                await db.HashSetAsync(sessionKey, hash);
                await db.StringSetAsync(eventKey, "1", TimeSpan.FromDays(7));
                await db.StreamAcknowledgeAsync("events:customer-sent-product", "workers", message.Id);
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}