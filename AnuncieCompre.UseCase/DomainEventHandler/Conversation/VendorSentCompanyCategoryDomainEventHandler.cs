using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentCompanyCategoryDomainEventFactory(IDatabase _db) : IDomainEventHandler<VendorSentCompanyCategoryDomainEvent>
{
    private readonly IDatabase db = _db;

    public async Task HandleAsync(VendorSentCompanyCategoryDomainEvent domainEvent)
    {  
        var json = JsonSerializer.Serialize(domainEvent.CompanyCategory);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("companyCategory", json),
        };

        await db.HashSetAsync(key, hash);
    }
}