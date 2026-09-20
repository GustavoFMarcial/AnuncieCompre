using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.TransitionAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Conversation.NodeValidators;

public class ValidationNodeValidator(IValueObjectValidator valueObjectValidator) : INodeValidator
{
    private readonly IValueObjectValidator ValueObjectValidator = valueObjectValidator;

    public NodeResult Validate(ConversationNode conversationNode, string message)
    {
        Result<ValueObject> result = ValueObjectValidator.Validate(message);

        if (!result.IsSuccess) return NodeResult.Failure(result.Message, conversationNode.Id);

        ConversationNodeTransition? transition = conversationNode.Transitions.FirstOrDefault();

        if (transition is null) return NodeResult.Failure("Opção inválida", conversationNode.Id);

        return NodeResult.Success(transition.TargetNodeId);
    }
}