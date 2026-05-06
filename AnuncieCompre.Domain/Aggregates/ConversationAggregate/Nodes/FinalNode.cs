using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class FinalNode : IConversationNode
{
    public string Id { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Dictionary<string, IConversationNode> Transitions { get; set ; } = [];
    // public string RecoupId { get; set; } = default!;
    // public string RecoupMessage { get; set; } = default!;
    public INodeValidator NodeValidator { get; set; } = default!;
    public IDomainEventFactory DomainEventFactory { get; set; } = default!;
}