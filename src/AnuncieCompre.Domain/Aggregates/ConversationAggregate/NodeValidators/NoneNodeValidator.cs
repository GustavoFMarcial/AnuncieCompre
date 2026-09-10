using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class NoneNodeValidator : INodeValidator
{
    public NodeResult Validate(ConversationNode conversationNode, string message)
    {
        return NodeResult.Success(conversationNode.Transitions["1"]);
    }
}