using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;


namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentQuantityDomainEventHandler(IDatabase _db) : BackgroundService
{
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:customer-sent-quantity", "workers", "customer-sent-quantity", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:customer-sent-quantity", "workers", "customer-sent-quantity", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];

                if (payload == null) continue;

                var domainEvent = JsonSerializer.Deserialize<CustomerSentQuantityDomainEvent>(payload);

                if (domainEvent == null) continue;

                string key = $"session:{domainEvent.Phone}";

                var hash = new HashEntry[]
                {
                    new("quantity", domainEvent.Quantity),
                };

                await db.HashSetAsync(key, hash);
                await db.StreamAcknowledgeAsync("events:customer-sent-quantity", "workers", message.Id);
            }
                
            await Task.Delay(1000, stoppingToken);
        }
    }
}
