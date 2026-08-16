namespace AnuncieCompre.Web.DTO;

public record CreateConversationNodeResponse
{
    public string Id { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string ValidationKind { get; set; } = default!;
}