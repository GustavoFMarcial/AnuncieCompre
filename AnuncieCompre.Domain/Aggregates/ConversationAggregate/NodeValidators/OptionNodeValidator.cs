using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class OptionNodeValidator(List<string> options) : INodeValidator
{
    public List<string> Options { get; set; } = options;

    public NodeResult Validate(IConversationNode conversationNode, string message)
    {
        foreach (string o in Options)
        {
            if (message == o) return NodeResult.Success(conversationNode.Transitions[o].Message, conversationNode.Transitions[o].Id);
        }

        return NodeResult.Failure("Opção inválida, escolha novamente", conversationNode.Id);
    }
}