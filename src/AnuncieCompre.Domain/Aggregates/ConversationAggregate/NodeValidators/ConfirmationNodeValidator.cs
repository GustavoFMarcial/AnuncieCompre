using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class ConfirmationNodeValidator(List<string> options) : INodeValidator
{
    private readonly List<string> Options = options;

    public NodeResult Validate(ConversationNode conversationNode, string message)
    {
        bool isValidOption = conversationNode.Transitions.TryGetValue(message, out Guid targetConversationNodeId);

        if (isValidOption is false) return NodeResult.Failure("Opção inválida, escolha novamente", conversationNode.Id);

        return NodeResult.Success(targetConversationNodeId);
    }
}