using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class ConversationNode
{
    public string Id { get; set; } = default!;
    public string[] Options { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Dictionary<string, ConversationNode> Transitions { get; set ; } = default!;
    public IValueObjectValidator ValueObjectValidator { get; set; } = default!;
    public IDomainEventFactory DomainEventFactory { get; set; } = default!;
    public string TempDataType { get; set; } = default!;
}