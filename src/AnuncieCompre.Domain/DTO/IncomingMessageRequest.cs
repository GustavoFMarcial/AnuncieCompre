namespace AnuncieCompre.Domain.DTO;

public class IncomingMessageRequest
{
    public string MessageId { get; set; } = string.Empty;
    public string SenderPhone { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool HasAttachments { get; set; }
}