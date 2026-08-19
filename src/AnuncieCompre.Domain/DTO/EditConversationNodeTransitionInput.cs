namespace AnuncieCompre.Domain.DTO;

public record EditConversationNodeTransitionInput
{
    public string Option { get; set; } = default!;
    public string TargetNodeId { get; set; } = default!;
}