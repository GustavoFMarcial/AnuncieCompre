namespace AnuncieCompre.Web.DTO;

public record EditConversationNodeResponse
{
    public Guid ConversationFlowId { get; set; } = default!;
    public Guid ConversationNodeId { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string ValidationKind { get; set; } = default!;
    public string ValueObjectValidator { get; set; } = default!;
    public string[]? Options { get; set; } = default!;
    public bool IsFinal { get; set; }
}