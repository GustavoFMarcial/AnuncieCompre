namespace AnuncieCompre.Domain.DTO;

public record Transiton
{
    public string Option { get; set; } = default!;
    public int TargetNodeNumber { get; set; }
    public Guid TargetNodeId { get; set; }
}