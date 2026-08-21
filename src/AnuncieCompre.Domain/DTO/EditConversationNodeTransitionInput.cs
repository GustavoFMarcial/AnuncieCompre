namespace AnuncieCompre.Domain.DTO;

public record EditConversationNodeTransitionInput
{
    public string Option { get; set; } = default!;
    public Guid TargetNodeId { get; set; } = default!;
}