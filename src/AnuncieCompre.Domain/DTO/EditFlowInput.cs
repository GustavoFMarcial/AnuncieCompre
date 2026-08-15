using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.DTO;

public record EditFlowInput
{
    public string Name { get; set; }  = default!;
    public string? Description { get; set; }
    public FlowStatus Status { get; set; }
}