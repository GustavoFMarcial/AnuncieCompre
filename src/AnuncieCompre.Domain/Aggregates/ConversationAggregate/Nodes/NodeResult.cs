using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Conversation.Nodes;

public class NodeResult : Result<ValueObject>
{
    public Guid NextStepId { get; set; } = default!;
    public bool ProcDomainEvent { get; set; }

    public static NodeResult Success(ValueObject value, string message, Guid nextStepId, bool procDomainevent = true)
    {
        NodeResult result = new()
        {
            IsSuccess = true,
            Message = message,
            Value = value,
            NextStepId = nextStepId,
            ProcDomainEvent = procDomainevent,
        };

        return result;
    }

    public static NodeResult Success(string message, Guid nextStepId, bool procDomainEvent = true)
    {
        NodeResult result = new()
        {
            IsSuccess = true,
            Message = message,
            NextStepId = nextStepId,
            ProcDomainEvent = procDomainEvent,
        };

        return result;
    }

    public static NodeResult Success(Guid nextStepId, string message)
    {
        NodeResult result = new()
        {
            IsSuccess = true,
            Message = message,
            NextStepId = nextStepId,
        };

        return result;
    }

    public static NodeResult Success(Guid nextStepId)
    {
        NodeResult result = new()
        {
            IsSuccess = true,
            NextStepId = nextStepId,
        };

        return result;
    }

    public static NodeResult Failure(string message, Guid nextStepId)
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

    internal static NodeResult Success(object id)
    {
        throw new NotImplementedException();
    }
}