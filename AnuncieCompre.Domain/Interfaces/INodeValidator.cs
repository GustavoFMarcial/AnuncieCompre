using AnuncieCompre.Domain.Conversation.Nodes;

namespace AnuncieCompre.Domain.Interfaces;

public interface INodeValidator
{
    public NodeResult Validate(IConversationNode conversationNode, string message);
}