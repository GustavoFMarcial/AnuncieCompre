using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Infra.Providers;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.UseCase.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(IConversationRepository _conversationRepository, IUserRepository _userRepository, IUnitOfWork _unitOfWork, ConversationFlowProvider _conversationFlowProvider) : IProcessIncomingMessage
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IUserRepository userRepository = _userRepository;
    private readonly ConversationFlowProvider conversationFlowProvider = _conversationFlowProvider;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    
    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Conversation? conversation = await conversationRepository.GetConversationByPhoneAsync(incomingMessage.SenderPhone);
        User ? user = await userRepository.GetUserByPhoneAsync(incomingMessage.SenderPhone);

        if (conversation is null || user is null)
        {
            conversation = Conversation.Create(Phone.Create(incomingMessage.SenderPhone).Value);
            user = User.Create(Phone.Create(incomingMessage.SenderPhone).Value);
            conversationRepository.Add(conversation);
            userRepository.Add(user);
        }

        ConversationNode awaitingRespondeNode = conversationFlowProvider.GetById(conversation.AwaitingResponseNodeId);

        ReadOnlyCollection<string> response = conversation.HandleMessage(incomingMessage.Content, awaitingRespondeNode, user);

        await unitOfWork.SaveChangesAsync();

        return response;
    }
}