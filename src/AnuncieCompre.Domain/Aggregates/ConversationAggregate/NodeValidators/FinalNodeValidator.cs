using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class FinalNodeValidator : INodeValidator
{
    public NodeResult Validate(IConversationNode conversationNode, string message)
    {
        return NodeResult.Success(conversationNode.Transitions["next"].Message, conversationNode.Transitions["next"].Id);
    }
}