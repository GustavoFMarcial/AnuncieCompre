using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class OptionNodeValidator(List<string> options) : INodeValidator
{
    private readonly List<string> Options = options;

    public NodeResult Validate(ConversationNode conversationNode, string message)
    {
        conversationNode.Transitions.TryGetValue(message, out ConversationNode? targetConversationNode);

        if (targetConversationNode is null) return NodeResult.Failure("Opção inválida, escolha novamente", conversationNode.Id);

        return NodeResult.Success(targetConversationNode.Id, targetConversationNode.Message);
    }
}