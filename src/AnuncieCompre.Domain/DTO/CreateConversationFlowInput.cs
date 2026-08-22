using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.DTO;

public record CreateConversationFlowInput
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public FlowStatus Status { get; set; } = default!;
}