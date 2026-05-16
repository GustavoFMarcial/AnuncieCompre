using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Conversation.Nodes;

public class NodeResult : Result<ValueObject>
{
    public string NextStepId { get; set; } = default!;

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

    public static NodeResult Failure(string message, string nextStepId)
    {
        NodeResult result = new()
        {
            IsSuccess = false,
            Message = message,
            Value = default!,
            NextStepId = nextStepId,
        };

        return result;
    }
}