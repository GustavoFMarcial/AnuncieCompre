using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserSentNameDomainEventHandler(IUnitOfWork _unitOfWork) : IDomainEventHandler<UserSentNameDomainEvent>
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserSentNameDomainEvent domainEvent)
    {
        domainEvent.User.SetName(domainEvent.Name);
        await unitOfWork.SaveChangesAsync();
    }
}