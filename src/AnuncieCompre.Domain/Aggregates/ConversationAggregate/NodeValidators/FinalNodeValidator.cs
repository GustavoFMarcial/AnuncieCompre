using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.TransitionAggregate;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class FinalNodeValidator : INodeValidator
{
    public NodeResult Validate(ConversationNode conversationNode, string message)
    {
        ConversationNodeTransition? transition = conversationNode.Transitions.FirstOrDefault();

        if (transition is null) return NodeResult.Failure("Opção inválida", conversationNode.Id);

        return NodeResult.Success(transition.TargetNodeId);
    }
}