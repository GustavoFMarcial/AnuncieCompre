using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Infra.Repositories.RedisRepo;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentTypeDomainEventHandler(RedisRepository _redisRepository) : IDomainEventHandler<UserSentTypeDomainEvent>
{
    private readonly RedisRepository redisRepository = _redisRepository;

    public async Task HandleAsync(UserSentTypeDomainEvent domainEvent)
    {
        var json = JsonSerializer.Serialize(domainEvent.UserType);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("type", json),
        };

        await redisRepository.Db.HashSetAsync(key, hash);
    }
}