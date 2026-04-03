using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class ConversationCreatedDomainEventHandler(IConversationRepository _conversationRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<ConversationCreatedDomainEvent>
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(ConversationCreatedDomainEvent domainEvent)
    {
        conversationRepository.Add(domainEvent.Conversation);
        await unitOfWork.SaveChangesAsync();
    }
}