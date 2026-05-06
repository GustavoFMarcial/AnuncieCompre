using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.NodeValidators;

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