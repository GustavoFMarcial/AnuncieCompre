using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class OptionValidationNodeValidator(List<string> options, IValueObjectValidator valueObjectValidator) : INodeValidator 
{
    private readonly List<string> Options = options;
    private readonly IValueObjectValidator valueObjectValidator = valueObjectValidator;

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
            if (message == o.ToString())
            {
                Result<ValueObject> result = valueObjectValidator.Validate(message);

                if (result.IsSuccess)
                {
                    return NodeResult.Success(conversationNode.Transitions[o].TargetNodeId);
                }
                else
                {
                    return NodeResult.Failure(result.Message, conversationNode.Id);
                }
            }
        }

        return NodeResult.Failure("Opção inválida", conversationNode.Id);
    }
}