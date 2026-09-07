namespace AnuncieCompre.Domain.DTO;

public record Transiton
{
    public string Option { get; set; } = default!;
    public Guid TargetNodeId { get; set; }
}