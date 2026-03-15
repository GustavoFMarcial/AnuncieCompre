using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler;

public class UserSentDataToRegisterDomainEventHandler(IUserRepository _userRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<UserSentDataToRegisterDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserSentDataToRegisterDomainEvent domainEvent)
    {
        User? user = await userRepository.GetByIdAsync(domainEvent.UserId);

        if (user is null) return;

        user
            .SetName(domainEvent.Name!)
            .SetEmail(domainEvent.Email!)
            .SetType(domainEvent.UserType);

        await unitOfWork.SaveChangesAsync();
    }
}