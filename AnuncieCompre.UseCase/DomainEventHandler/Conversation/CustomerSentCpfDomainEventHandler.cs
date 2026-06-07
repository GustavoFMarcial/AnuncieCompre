using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentCpfDomainEventHandler(IDatabase _db) : BackgroundService
{
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:customer-sent-cpf", "workers", "customer-sent-cpf", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:customer-sent-cpf", "workers", "customer-sent-cpf", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];

                if (payload == null) continue;

                var domainEvent = JsonSerializer.Deserialize<CustomerSentCpfDomainEvent>(payload);

                if (domainEvent == null) continue;

                string key = $"session:{domainEvent.Phone}";

                var hash = new HashEntry[]
                {
                    new("cpf", domainEvent.cpf),
                };

                await db.HashSetAsync(key, hash);
                await db.StreamAcknowledgeAsync("events:customer-sent-cpf", "workers", message.Id);
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}