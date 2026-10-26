namespace AnuncieCompre.Domain.DTO;

public record RealTimeMarkAsReadMessageDTO
{
    public Guid MessageId { get; set; }
    public bool IsRead { get; set; }
}