using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentCnpjDomainEventFactory(IDatabase _db) : IDomainEventHandler<VendorSentCnpjDomainEvent>
{
    private readonly IDatabase db = _db;

    public async Task HandleAsync(VendorSentCnpjDomainEvent domainEvent)
    {  
        var json = JsonSerializer.Serialize(domainEvent.CNPJ);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("cnpj", json),
        };

        await db.HashSetAsync(key, hash);
    }
}