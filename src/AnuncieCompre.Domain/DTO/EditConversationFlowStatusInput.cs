using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.DTO;

public record EditConversationFlowStatusInput
{
    public FlowStatus Status { get; set; }
}