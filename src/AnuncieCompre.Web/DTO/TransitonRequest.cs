namespace AnuncieCompre.Web.DTO;

public record TransitonRequest
{
    public string Option { get; set; } = default!;
    public Guid TargetNodeId { get; set; }
}