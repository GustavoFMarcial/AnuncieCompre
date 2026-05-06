using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

namespace AnuncieCompre.Domain.Interfaces;

public interface INodeValidator
{
    public NodeResult Validate(IConversationNode conversationNode, string message);
}