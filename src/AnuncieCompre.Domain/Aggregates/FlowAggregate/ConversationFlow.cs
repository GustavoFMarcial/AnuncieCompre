using System.Diagnostics;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.FlowAggregate;

public class ConversationFlow : BaseEntity
{
    public Name Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public FlowStatus Status { get; private set; } = default!;
    public List<ConversationNode> Nodes { get; private set; } = [];

    private ConversationFlow() {}

    private ConversationFlow(Name name, FlowStatus status, string description = default!)
    {
        Name = name;
        Description = description;
        Status = status;
    }

    public static Result<ConversationFlow> Create(string name, string description, FlowStatus status)
    {
        Result<Name> result = Name.Create(name);

        if (result.IsSuccess is false) return Result<ConversationFlow>.Failure(result.Message);

        ConversationFlow flow = new(result.Value, status, description);
        return Result<ConversationFlow>.Success(flow, "Flow criado com sucesso");
    }

    public Result EditFlow(EditConversationFlowInput input)
    {
        Result<Name> result = Name.Create(input.Name);

        if (!result.IsSuccess) return Result.Failure(result.Message);

        Name = result.Value;
        Description = input.Description;
        Status = input.Status;

        return Result.Success("Flow editado com sucesso");
    }
}