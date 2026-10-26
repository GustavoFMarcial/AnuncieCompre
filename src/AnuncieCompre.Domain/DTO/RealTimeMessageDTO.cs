using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.DTO;

public record RealTimeMessageDTO
{
    public Guid ConversationId { get; set; }
    public MessageSenderType SenderType { get; set; }
    public MessageDirection Direction { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = default!;
    public bool IsRead { get; set; }
}