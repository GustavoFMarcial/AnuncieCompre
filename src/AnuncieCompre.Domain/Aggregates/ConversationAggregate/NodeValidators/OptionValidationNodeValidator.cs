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
        conversationNode.Transitions.TryGetValue(message, out ConversationNode? targetConversationNode);

        if (targetConversationNode is null) return NodeResult.Failure("Opção inválida, escolha novamente", conversationNode.Id);

        Result<ValueObject> result = valueObjectValidator.Validate(message);

        if (result.IsSuccess)
        {
            return NodeResult.Success(targetConversationNode.Id, targetConversationNode.Message);
        }
        else
        {
            return NodeResult.Failure(result.Message, conversationNode.Id);
        }
    }
}