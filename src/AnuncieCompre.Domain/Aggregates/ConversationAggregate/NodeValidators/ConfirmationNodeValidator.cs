using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class ConfirmationNodeValidator(List<string> options) : INodeValidator
{
    private readonly List<string> Options = options;

    public NodeResult Validate(ConversationNode conversationNode, string message)
    {
        if (message == Options[0])
        {
            return NodeResult.Success(conversationNode.Transitions[0].TargetNodeId);
        }
        else if (message == Options[1])
        {
            return NodeResult.Success(conversationNode.Transitions[1].TargetNodeId);   
        }

        return NodeResult.Failure("Opção inválida, escolha novamente", conversationNode.Id);
    }
}