using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.UseCase.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(IConversationRepository _conversationRepository, IUnitOfWork _unitOfWork) : IProcessIncomingMessage
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    
    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Conversation? conversation = await conversationRepository.GetConversationByPhoneAsync(incomingMessage.SenderPhone) ?? Conversation.Create(Phone.Create(incomingMessage.SenderPhone).Value);

        ReadOnlyCollection<string> response = conversation.HandleMessage(incomingMessage.Content);

        await unitOfWork.SaveChangesAsync();

        return response;
    }
}