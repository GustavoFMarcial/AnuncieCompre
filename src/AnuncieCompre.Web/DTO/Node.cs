using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Web.DTO;

public record Node
{
    public Guid Id { get; set; }
    public string Message { get; set; } = default!;
    public string ValidationKind { get; set; } = default!;
    public string ValueObjectValidator { get; set; } = default!;
    public List<string> Options { get; set; } = [];
    public List<Transiton> Transitions { get; set; } = [];
    public bool IsFinal { get; set; }
 }