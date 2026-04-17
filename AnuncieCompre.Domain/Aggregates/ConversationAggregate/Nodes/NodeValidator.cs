using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class NodeValidator
{
    public static NodeResult Validate(ConversationNode awaitingResponseNode, string message)
    {
        IResultValueObject result = awaitingResponseNode.ValueObjectValidator.Validate(message);

        if (!result.IsSuccess)
        {
            return (NodeResult)NodeResult.Failure(result.Message);
        }

        string nextStepId = awaitingResponseNode.Transitions[awaitingResponseNode.ValueObjectValidator is OptionValidator ? message : "next"].Id;
        string nextMessage = awaitingResponseNode.Transitions[nextStepId].Message;
        
        return NodeResult.Success(result.Value!, nextMessage, nextStepId);
    }
}