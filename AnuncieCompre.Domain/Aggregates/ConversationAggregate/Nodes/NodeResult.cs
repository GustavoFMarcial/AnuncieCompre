using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class NodeResult(bool isSuccess, string message, string? nextStep = null, ValueObject? valueObject = null)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public string Message { get; set; } = message;
    public string? NextStep { get; set; } = nextStep;
    public ValueObject? Value { get; set; } = valueObject;
}