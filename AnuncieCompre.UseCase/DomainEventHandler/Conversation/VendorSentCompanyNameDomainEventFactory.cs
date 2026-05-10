using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentCompanyNameDomainEventFactory(IDatabase _db) : IDomainEventHandler<VendorSentCompanyNameDomainEvent>
{
    private readonly IDatabase db = _db;

    public async Task HandleAsync(VendorSentCompanyNameDomainEvent domainEvent)
    {  
        var json = JsonSerializer.Serialize(domainEvent.Name);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("companyName", json),
        };

        await db.HashSetAsync(key, hash);
    }
}