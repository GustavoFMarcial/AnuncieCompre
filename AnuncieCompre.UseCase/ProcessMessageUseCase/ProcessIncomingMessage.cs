using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Infra.Providers;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

namespace AnuncieCompre.UseCase.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(IConversationRepository _conversationRepository, IUnitOfWork _unitOfWork, ConversationFlowProvider _conversationFlowProvider) : IProcessIncomingMessage
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly ConversationFlowProvider conversationFlowProvider = _conversationFlowProvider;
    
    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Conversation? conversation = await conversationRepository.GetConversationByPhoneAsync(incomingMessage.SenderPhone);

        if (conversation is null)
        {
            conversation = Conversation.Create(Phone.Create(incomingMessage.SenderPhone).Value);
            conversationRepository.Add(conversation);
        }

        ConversationNode awaitingRespondeNode = conversationFlowProvider.GetById(conversation.AwaitingResponseNodeId);

        ReadOnlyCollection<string> response = conversation.HandleMessage(incomingMessage.Content, awaitingRespondeNode);

        await unitOfWork.SaveChangesAsync();

        return response;
    }
}