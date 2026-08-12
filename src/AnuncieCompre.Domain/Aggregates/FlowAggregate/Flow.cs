using System.Diagnostics;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.FlowAggregate;

public class Flow : BaseEntity
{
    public Name Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public FlowStatus Status { get; private set; } = default!;
    public List<Node>? Nodes { get; private set; } = [];

    private Flow() {}

    private Flow(Name name, FlowStatus status, string description = default!)
    {
        Name = name;
        Description = description;
        Status = status;
    }

    public static Result<Flow> Create(string name, string description, FlowStatus status)
    {
        Result<Name> result = Name.Create(name);

        if (result.IsSuccess is false) return Result<Flow>.Failure(result.Message);

        Flow flow = new(result.Value, status, description);
        return Result<Flow>.Success(flow, "Flow criado com sucesso");
    }
}