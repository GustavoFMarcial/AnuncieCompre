using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class NodeValidator
{
    public static NodeResult Validate(ConversationNode awaitingResponseNode, string message)
    {
        IResultValueObject result = awaitingResponseNode.ValueObjectValidator.Validate(message);
        string nextStepId;
        string nextMessage;

        if (!result.IsSuccess)
        {
            return NodeResult.Failure(result.Message);
        }

        if (awaitingResponseNode.ValueObjectValidator is UserTypeValidator)
        {
            nextStepId = awaitingResponseNode.Transitions[message].Id;
            nextMessage = awaitingResponseNode.Transitions[message].Message;
        }
        else
        {
            nextStepId = awaitingResponseNode.Transitions[awaitingResponseNode.ValueObjectValidator is OptionValidator ? message : "next"].Id;
            nextMessage = awaitingResponseNode.Transitions[awaitingResponseNode.ValueObjectValidator is OptionValidator ? message : "next"].Message;
        }
        
        return NodeResult.Success(result.Value!, nextMessage, nextStepId);
    }
}