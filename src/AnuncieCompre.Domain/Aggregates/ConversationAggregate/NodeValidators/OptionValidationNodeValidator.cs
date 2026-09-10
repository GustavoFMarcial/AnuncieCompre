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
        bool isValidOption = conversationNode.Transitions.TryGetValue(message, out Guid targetConversationNodeId);

        if (isValidOption is false) return NodeResult.Failure("Opção inválida, escolha novamente", conversationNode.Id);

        Result<ValueObject> result = valueObjectValidator.Validate(message);

        if (result.IsSuccess)
        {
            return NodeResult.Success(targetConversationNodeId);
        }
        else
        {
            return NodeResult.Failure(result.Message, conversationNode.Id);
        }
    }
}