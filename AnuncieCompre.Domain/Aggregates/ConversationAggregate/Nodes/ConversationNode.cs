using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class ConversationNode
{
    public string[] Options { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Dictionary<string, ConversationNode> Transitions { get; set ; } = default!;
    public IValueObjectValidator ValueObjectValidator { get; set; } = default!;
    public IDomainEvent DomainEventType { get; set; } = default!;
    public string TempDataType { get; set; } = default!;
}