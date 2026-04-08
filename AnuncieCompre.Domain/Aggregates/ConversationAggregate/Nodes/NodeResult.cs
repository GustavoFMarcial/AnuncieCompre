using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class NodeResult : Result<ValueObject>
{
    public string? NextStep { get; set; }

    public static NodeResult Success(ValueObject value, string message, string nextStep)
    {
        NodeResult result = new()
        {
            IsSuccess = true,
            Message = message,
            Value = value,
            NextStep = nextStep,
        };

        return result;
    }
}