using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.MessageAggregate;

public class Message : BaseEntity
{
    public Guid ConversationId { get; private set; }
    public ConversationAggregate.Conversation Conversation { get; private set; } = default!;
    public string Text { get; private set; } = default!;
    public MessageSenderType SenderType { get; private set; }
    public MessageDirection Direction { get; private set; }
}