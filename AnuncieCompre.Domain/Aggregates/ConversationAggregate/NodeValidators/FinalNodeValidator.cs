using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.NodeValidators;

public class FinalNodeValidator : INodeValidator
{
    public NodeResult Validate(IConversationNode conversationNode, string message)
    {
        return NodeResult.Success(conversationNode.Transitions["next"].Message, conversationNode.Transitions["next"].Id);
    }
}