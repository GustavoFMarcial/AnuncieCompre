namespace AnuncieCompre.Domain.Interfaces;

public interface IConversationNode
{
    public string Id { get; set; }
    public string Message { get; set; }
    public Dictionary<string, IConversationNode> Transitions { get; set; }
    public INodeValidator NodeValidator { get; set; }
    public IDomainEventFactory DomainEventFactory { get; set; }
}