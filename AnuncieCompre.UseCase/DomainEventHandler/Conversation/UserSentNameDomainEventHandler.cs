using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentNameDomainEventHandler(/*IUserRepository _userRepository,*/ IUnitOfWork _unitOfWork) : IDomainEventHandler<UserSentNameDomainEvent>
{
    // private readonly IUserRepository userRepository = _userRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserSentNameDomainEvent domainEvent)
    {
        // User? user = await userRepository.GetByIdAsync(domainEvent.User.Id);

        // if (user is null) return;
        
        domainEvent.User.SetName(domainEvent.Name);
        await unitOfWork.SaveChangesAsync();
    }
}