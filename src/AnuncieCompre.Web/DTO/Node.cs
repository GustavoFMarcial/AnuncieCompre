using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Web.DTO;

public record Node
{
    public Guid Id { get; set; }
    public string Message { get; set; } = default!;
    public string ValidationKind { get; set; } = default!;
    public string ValueObjectValidator { get; set; } = default!;
    public string[]? Options { get; set; } = [];
    public List<Transition> Transitions { get; set; } = [];
    public bool IsFinal { get; set; }
 }