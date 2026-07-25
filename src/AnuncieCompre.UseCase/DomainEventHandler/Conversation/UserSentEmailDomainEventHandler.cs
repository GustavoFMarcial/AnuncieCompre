using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserSentEmailDomainEventHandler(IUnitOfWork _unitOfWork) : IDomainEventHandler<UserSentEmailDomainEvent>
{
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserSentEmailDomainEvent domainEvent)
    {
        domainEvent.User.SetEmail(domainEvent.Email);
        await unitOfWork.SaveChangesAsync();
    }
}