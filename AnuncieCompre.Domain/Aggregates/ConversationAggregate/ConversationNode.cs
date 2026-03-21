using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class ConversationNode
{
    public bool HasValidation { get; set; } = false;
    public Type ValidationType { get; set; } = default!;
    public bool HasDomainEvent { get; set; } = false;
    public IDomainEvent DomainEventType { get; set; } = default!;
    public bool HasTempData { get; set; } = false;
    public string TempDataType { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Dictionary<string, ConversationNode> Transitions { get; set; } = default!;
}