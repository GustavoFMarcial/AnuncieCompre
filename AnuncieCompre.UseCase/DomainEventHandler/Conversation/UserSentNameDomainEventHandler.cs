using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Infra.Repositories.RedisRepo;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentNameDomainEventHandler(RedisRepository _redisRepository) : IDomainEventHandler<UserSentNameDomainEvent>
{
    private readonly RedisRepository redisRepository = _redisRepository;

    public async Task HandleAsync(UserSentNameDomainEvent domainEvent)
    {
        var json = JsonSerializer.Serialize(domainEvent.Name);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("name", json),
        };

        await redisRepository.Db.HashSetAsync(key, hash);
    }
}