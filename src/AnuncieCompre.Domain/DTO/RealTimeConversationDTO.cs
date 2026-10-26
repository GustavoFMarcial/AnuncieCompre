using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.DTO;

public record RealTimeConversationDTO
{
    public Guid ConversationId { get; set; }
    public ConversationAttendant Attendant { get; set; }
    public ConversationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}