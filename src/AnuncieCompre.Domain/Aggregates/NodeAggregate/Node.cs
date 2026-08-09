using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.NodeAggregate;

public class Node : BaseEntity
{
    public string Message { get; private set; } = default!;
    public INodeValidator? NodeValidator { get; private set; }
    public IValueObjectValidator? ValueObjectValidator { get; private set; }
    public string[] Options { get; private set; } = [];
    public (string options, string targetNodeId)[] Transitions { get; private set; } = [];
    public bool IsFinal { get; private set; }
}