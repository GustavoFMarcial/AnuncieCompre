namespace AnuncieCompre.Web.DTO;

public record Node
{
    public Guid Id { get; set; }
    public string Message { get; set; } = default!;
    public string ValidationKind { get; set; } = default!;
    public string ValueObjectValidator { get; set; } = default!;
    public string[]? Options { get; set; } = [];
    public (string option, string targetNodeId)[] Transitions { get; set; } = [];
    public bool IsFinal { get; set; }
 }