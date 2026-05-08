using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Infra.Repositories.RedisRepo;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserConfirmedRegistrationDomainEventHandler(RedisRepository _redisRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<UserConfirmedRegistrationDomainEvent>
{
    private readonly RedisRepository redisRepository = _redisRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserConfirmedRegistrationDomainEvent domainEvent)
    {
        string key = $"user{domainEvent.User.Phone.Value}";
        var entries = await redisRepository.Db.HashGetAllAsync(key);

        var data = entries.ToDictionary(
            x => x.Name.ToString(),
            x => x.Value.ToString()
        );

        var name = JsonSerializer.Deserialize<Name>(data["name"]);
        var email = JsonSerializer.Deserialize<Email>(data["email"]);
        var type = JsonSerializer.Deserialize<UserType>(data["type"]);

        if (name is null || email is null || type is null) return;

        domainEvent.User
            .SetName(name)
            .SetEmail(email)
            .SetUserType(type);

        await redisRepository.Db.KeyDeleteAsync(key);
        await unitOfWork.SaveChangesAsync();
    }
}