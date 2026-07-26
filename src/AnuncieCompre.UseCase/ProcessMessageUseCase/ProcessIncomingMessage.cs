using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Infra.Providers;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using StackExchange.Redis;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Infra.Repositories.MessageRepo;

namespace AnuncieCompre.UseCase.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(IUserRepository _userRepository, IConversationRepository _conversationRepository, IMessageRepository _messageRepository, ConversationFlowProvider _conversationFlowProvider, IUnitOfWork _unitOfWork) : IProcessIncomingMessage
{
    private readonly IUserRepository userRepository = _userRepository;
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IMessageRepository messageRepository = _messageRepository;
    private readonly ConversationFlowProvider conversationFlowProvider = _conversationFlowProvider;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        User? user = await userRepository.GetUserByPhoneAsync(incomingMessage.SenderPhone);

        if (user is null)
        {
            user = User.Create(Phone.Create(incomingMessage.SenderPhone).Value);
            userRepository.Add(user);
        }

        Conversation? conversation = await conversationRepository.GetOpenConversationByUserIdAsync(user.Id);

        if (conversation is null)
        {
            conversation = Conversation.Create(user);
            conversationRepository.Add(conversation);
        }

        IConversationNode awaitingRespondeNode = conversationFlowProvider.GetById(conversation.AwaitingResponseNodeId);

        ReadOnlyCollection<string> response = conversation.HandleMessage(awaitingRespondeNode, incomingMessage.Content, user);

        Message userMessage = Message.Create(conversation, incomingMessage.Content, MessageSenderType.Customer, MessageDirection.Incoming);
        Message chatMessage = Message.Create(conversation, response[0], MessageSenderType.Bot, MessageDirection.Outgoing);
        messageRepository.Add(userMessage);
        messageRepository.Add(chatMessage);

        await unitOfWork.SaveChangesAsync();
        return response;
    }
}