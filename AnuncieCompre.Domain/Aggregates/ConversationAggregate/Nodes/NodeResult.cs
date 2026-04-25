using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class NodeResult : Result<ValueObject>
{
    public string? NextStepId { get; set; }

    public static NodeResult Success(ValueObject value, string message, string nextStepId)
    {
        NodeResult result = new()
        {
            IsSuccess = true,
            Message = message,
            Value = value,
            NextStepId = nextStepId,
        };

        return result;
    }

    public static NodeResult Success(string message, string nextStepId)
    {
        NodeResult result = new()
        {
            IsSuccess = true,
            Message = message,
            NextStepId = nextStepId,
        };

        return result;
    }

    public new static NodeResult Failure(string message)
    {
        NodeResult result = new()
        {
            IsSuccess = false,
            Message = message,
            Value = default!,
        };

        return result;
    }
}