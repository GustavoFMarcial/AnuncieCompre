using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class ValidationNode : IConversationNode
{
    public string Id { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Dictionary<string, IConversationNode> Transitions { get; set ; } = [];
    public IDomainEventFactory DomainEventFactory { get; set; } = default!;
    public INodeValidator NodeValidator { get; set; } = default!;
}