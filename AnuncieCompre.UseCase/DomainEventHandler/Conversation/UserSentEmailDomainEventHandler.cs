using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class UserSentEmailDomainEventHandler(/*IUserRepository _userRepository,*/ IUnitOfWork _unitOfWork) : IDomainEventHandler<UserSentEmailDomainEvent>
{
    // private readonly IUserRepository userRepository = _userRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(UserSentEmailDomainEvent domainEvent)
    {
        // User? user = await userRepository.GetByIdAsync(domainEvent.User.Id);

        // if (user is null) return;
        
        domainEvent.User.SetEmail(domainEvent.Email);
        await unitOfWork.SaveChangesAsync();
    }
}