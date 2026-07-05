using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class ConfirmationNodeValidator(List<string> options) : INodeValidator
{
    private readonly List<string> Options = options;

    public NodeResult Validate(IConversationNode conversationNode, string message)
    {
        if (message == Options[0])
        {
            return NodeResult.Success(conversationNode.Transitions[message].Message, conversationNode.Transitions[message].Id);
        }
        else if (message == Options[1])
        {
            return NodeResult.Success(conversationNode.Transitions[message].Message, conversationNode.Transitions[message].Id, false);
        }

        return NodeResult.Failure("Opção inválida, escolha novamente", conversationNode.Id);
    }
}