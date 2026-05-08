using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.Infra.Repositories.RedisRepo;

public class RedisRepository(IConnectionMultiplexer redis)
{
    public IDatabase Db { get; private set; } = redis.GetDatabase();
}