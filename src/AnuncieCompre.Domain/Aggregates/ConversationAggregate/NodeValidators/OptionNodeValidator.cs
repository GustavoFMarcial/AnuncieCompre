using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class OptionNodeValidator(List<string> options) : INodeValidator
{
    private readonly List<string> Options = options;

    public NodeResult Validate(ConversationNode conversationNode, string message)
    {
        List<int> options = [];

        foreach (string o in Options)
        {
            _ = int.TryParse(o, out int result);
            options.Add(result);
        }

        foreach (int o in options)
        {
            if (message == o.ToString()) return NodeResult.Success(conversationNode.Transitions[o].TargetNodeId);
        }

        return NodeResult.Failure("Opção inválida, escolha novamente", conversationNode.Id);
    }
}