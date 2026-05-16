using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.Nodes;

public class OptionNode : IConversationNode
{
    public string Id { get; set; } = default!;
    public string Message { get; set; } = default!;
    public Dictionary<string, IConversationNode> Transitions { get; set; } = [];
    public List<IDomainEventFactory> DomainEventFactory { get; set; } = [];
    public INodeValidator NodeValidator { get; set; } = default!;
}