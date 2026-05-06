using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentTypeDomainEventHandler(/*IUserRepository _userRepository,*/ IUnitOfWork _unitOfWork) : IDomainEventHandler<UserSentTypeDomainEvent>
{
    // private readonly IUserRepository userRepository = _userRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserSentTypeDomainEvent domainEvent)
    {
        // User? user = await userRepository.GetByIdAsync(domainEvent.User.Id);

        // if (user is null) return;
        
        domainEvent.User.SetUserType(domainEvent.UserType);
        await unitOfWork.SaveChangesAsync();
    }
}