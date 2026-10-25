using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Aggregates.MessageProviderReferenceAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;
using StackExchange.Redis;

namespace AnuncieCompre.Application.UseCases.Conversations;

public class SendMessage(
    IConversationRepository _conversationRepository, 
    IMessageRepository _messageRepository, 
    IMessageSender _messageSender, 
    IMessageProviderReferenceRepository _messageProvicerReferenceRepository,
    IRealtimeNotifier _realTimeNotifier,
    IUnitOfWork _unitOfWork)
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IMessageRepository messageRepository = _messageRepository;
    private readonly IMessageSender messageSender = _messageSender;
    private readonly IMessageProviderReferenceRepository messageProviderReferenceRepository = _messageProvicerReferenceRepository;
    private readonly IRealtimeNotifier realTimeNotifier = _realTimeNotifier;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid id, string text)
    {
        Conversation? conversation = await conversationRepository.GetConversationByIdWithCustomerAsync(id);

        if (conversation is null) return Result.Failure("Conversation não encontrada");
        
        Message message = Message.Create(conversation, text, MessageSenderType.Operator, MessageDirection.Outgoing);
        messageRepository.Add(message);

        Result<MessageProviderReference> result = await messageSender.SendMessageAsync(message);

        if (result.IsSuccess)
        {
            messageProviderReferenceRepository.Add(result.Value);
        }

        await unitOfWork.SaveChangesAsync();
        await realTimeNotifier.NotifyNewMessageAsync(conversation.Id, message);
        return Result.Success(result.Message);
    }
}