using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.UseCase.Services;

namespace AnuncieCompre.UseCase.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(IConversationRepository _conversationRepository, IUnitOfWork _unitOfWork, ValidateUserMessage _validateUserMessage) : IProcessIncomingMessage
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly ValidateUserMessage validateUserMessage = _validateUserMessage;
    
    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Conversation? conversation = await conversationRepository.GetConversationByPhoneAsync(incomingMessage.SenderPhone) ?? Conversation.Create(VOPhone.Create(incomingMessage.SenderPhone).Value);
        bool messageValidation = validateUserMessage.Handle(conversation.AwaitingResponseNode?.Options, incomingMessage.Content);

        ReadOnlyCollection<string> response = conversation.HandleMessage(incomingMessage, messageValidation);

        await unitOfWork.SaveChangesAsync();

        return response;
    }
}