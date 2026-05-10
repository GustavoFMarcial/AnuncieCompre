using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerSentCpfDomainEventHandler(IDatabase _db) : IDomainEventHandler<CustomerSentCpfDomainEvent>
{
    private readonly IDatabase db = _db;

    public async Task HandleAsync(CustomerSentCpfDomainEvent domainEvent)
    {
        var json = JsonSerializer.Serialize(domainEvent.CPF);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("cpf", json),
        };

        await db.HashSetAsync(key, hash);
    }
}