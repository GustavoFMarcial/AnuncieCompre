using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class UserSentEmailDomainEventHandler(IUserRepository _userRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<UserSentEmailDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserSentEmailDomainEvent domainEvent)
    {
        User? user = await userRepository.GetUserByPhoneAsync(domainEvent.Phone.Value);

        if (user is null) return;

        user.SetEmail(domainEvent.Email);
        await unitOfWork.SaveChangesAsync();
    }
}