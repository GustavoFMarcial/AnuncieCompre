using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.Infra.Repositories.RedisRepo;

public class RedisRepository(IConnectionMultiplexer redis) : IRedisRepository
{
    private readonly IDatabase db = redis.GetDatabase();
}