using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class OptionValidationNodeValidator(List<string> options, IValueObjectValidator valueObjectValidator) : INodeValidator
{
    private readonly List<string> Options = options;
    private readonly IValueObjectValidator valueObjectValidator = valueObjectValidator;

    public NodeResult Validate(IConversationNode conversationNode, string message)
    {
        foreach (string o in Options)
        {
            if (message == o)
            {
                IResultValueObject result = valueObjectValidator.Validate(message);

                if (result.IsSuccess)
                {
                    return NodeResult.Success(result.Value!, conversationNode.Transitions[o].Message, conversationNode.Transitions[o].Id);
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