using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Infra.Repositories.RedisRepo;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentEmailDomainEventHandler(RedisRepository _redisRepository) : IDomainEventHandler<UserSentEmailDomainEvent>
{
    private readonly RedisRepository redisRepository = _redisRepository;

    public async Task HandleAsync(UserSentEmailDomainEvent domainEvent)
    {
        var json = JsonSerializer.Serialize(domainEvent.Email);
        string key = $"user:{domainEvent.User.Phone.Value}";

        var hash = new HashEntry[]
        {
            new("email", json),
        };

        await redisRepository.Db.HashSetAsync(key, hash);
    }
}