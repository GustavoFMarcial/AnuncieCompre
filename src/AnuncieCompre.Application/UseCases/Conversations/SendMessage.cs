using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.UseCases.Conversations;

public class SendMessage(IConversationRepository _conversationRepository, IMessageRepository _messageRepository, IMessageSender _messageSender, IUnitOfWork _unitOfWork)
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IMessageRepository messageRepository = _messageRepository;
    private readonly IMessageSender messageSender = _messageSender;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid id, string text)
    {
        Conversation? conversation = await conversationRepository.GetConversationByIdWithUserAsync(id);

        if (conversation is null) return Result.Failure("Conversation não encontrada");

        await messageSender.SendMessageAsync(conversation.User.Phone.Value, text);

        Message message = Message.Create(conversation, text, Domain.Enums.MessageSenderType.Operator, Domain.Enums.MessageDirection.Outgoing);
        messageRepository.Add(message);
        await unitOfWork.SaveChangesAsync();
        return Result.Success("Mensagem enviada com sucesso");
    }
}