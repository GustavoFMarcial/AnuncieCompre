using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;

namespace AnuncieCompre.Application.UseCases.Conversations;

public class MarkMessagesAsRead(IMessageRepository _messageRepository,  IUnitOfWork _unitOfWork)
{
    private readonly IMessageRepository messageRepository = _messageRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task Handle(Guid conversationId)
    {
        List<Message> messages = await messageRepository.GetMessagesNotReadByConversationIdAsync(conversationId);

        foreach (Message m in messages)
        {
            m.MarkAsRead();
        }

        await unitOfWork.SaveChangesAsync();
    }
}