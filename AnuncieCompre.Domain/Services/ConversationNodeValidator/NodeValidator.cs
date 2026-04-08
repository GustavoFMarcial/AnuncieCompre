using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ConversationNodeValidator;

public class NodeValidator
{
    public static NodeResult Validate(ConversationNode awaitingResponseNode, ConversationNode activeNode, string message)
    {
        IResultValueObject result = awaitingResponseNode.ValueObjectValidator.Validate(message);

        if (!result.IsSuccess)
        {
            return (NodeResult)NodeResult.Failure(result.Message);
        }

        return NodeResult.Success(result.Value!, activeNode.Message, awaitingResponseNode.Options == null ? message : "next");
    }
}