using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class ConversationNode
{
    public bool HasValidation { get; set; }
    public Type ValidationType { get; set; } = default!;
    public bool HasDomainEvent { get; set; }
    public IDomainEvent DomainEventType { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Dictionary<string, ConversationNode> Transitions { get; set; } = default!;
}