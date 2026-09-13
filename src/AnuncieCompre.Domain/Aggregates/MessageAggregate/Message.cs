using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.MessageAggregate;

public class Message : BaseEntity
{
    public Guid ConversationId { get; private set; }
    public ConversationAggregate.Conversation Conversation { get; private set; } = default!;
    public string Text { get; private set; } = default!;
    public MessageFailureType FailureType { get; private set; }
    public MessageStatus MessageStatus { get; private set; }
    public int RetryAttempts { get; private set; }
    public int MaxRetryAttempts { get; private set; } = 5;
    public MessageSenderType SenderType { get; private set; }
    public MessageDirection Direction { get; private set; }

    private Message() { }

    private Message(ConversationAggregate.Conversation conversation, string text, MessageSenderType senderType, MessageDirection direction)
    {
        ConversationId = conversation.Id;
        Conversation = conversation;
        Text = text;
        SenderType = senderType;
        Direction = direction;
    }

    public static Message Create(ConversationAggregate.Conversation conversation, string text, MessageSenderType senderType, MessageDirection direction)
    {
        return new(conversation, text, senderType, direction);
    }

    public void SetFailureType(MessageFailureType failureType)
    {
        FailureType = failureType;
    }

    public void IncrementRetryAttempts()
    {
        if (RetryAttempts >= MaxRetryAttempts) return;
        RetryAttempts += 1;
    }
}