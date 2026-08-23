using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.DTO;

public record EditConversationFlowInput
{
    public string Name { get; set; }  = default!;
    public string? Description { get; set; }
}