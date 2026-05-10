using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class ValidationNodeValidator(IValueObjectValidator valueObjectValidator) : INodeValidator
{
    public IValueObjectValidator ValueObjectValidator = valueObjectValidator;

    public NodeResult Validate(IConversationNode conversationNode, string message)
    {
        IResultValueObject result = ValueObjectValidator.Validate(message);

        if (!result.IsSuccess)
        {
            return NodeResult.Failure(result.Message);
        }

        return NodeResult.Success(result.Value!, conversationNode.Transitions["next"].Message, conversationNode.Transitions["next"].Id);
    }
}