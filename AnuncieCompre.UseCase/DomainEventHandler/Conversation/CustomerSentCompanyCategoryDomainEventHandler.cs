using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentCompanyCategoryDomainEventHandler(IDatabase _db) : BackgroundService
{
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:customer-sent-company-category", "workers", "customer-sent-company-category", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:customer-sent-company-category", "workers", "customer-sent-company-category", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];

                if (payload == null) continue;

                var domainEvent = JsonSerializer.Deserialize<CustomerSentCompanyCategoryDomainEvent>(payload);

                if (domainEvent == null) continue;

                string key = $"session:{domainEvent.Phone}";

                domainEvent.CompanyCategory = domainEvent.CompanyCategory switch
                {
                    "Autopeça" => "1",
                    "MaterialdeConstrução" => "2",
                    "Automóvel" => "3",
                    "AparelhosEletrônicos" => "4",
                    "Eletrodomésticos" => "5",
                    _ => "6",
                };
                ; 

                var json = JsonSerializer.Serialize(domainEvent.CompanyCategory);

                var hash = new HashEntry[]
                {
                    new("companyCategory", json),
                };

                await db.HashSetAsync(key, hash);
                await db.StreamAcknowledgeAsync("events:customer-sent-company-category", "workers", message.Id);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}