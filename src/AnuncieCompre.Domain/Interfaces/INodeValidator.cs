using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.Nodes;

namespace AnuncieCompre.Domain.Interfaces;

public interface INodeValidator
{
    public NodeResult Validate(ConversationNode conversationNode, string message);
}