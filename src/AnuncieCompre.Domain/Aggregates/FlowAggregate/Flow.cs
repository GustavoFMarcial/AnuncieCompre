using System.Diagnostics;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.FlowAggregate;

public class Flow : BaseEntity
{
    public Name Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public FlowStatus Status { get; private set; } = default!;
    public List<Node>? Nodes { get; private set; } = [];

    private Flow() {}

    private Flow(Name name, string description, FlowStatus status)
    {
        Name = name;
        Description = description;
        Status = status;
    }

    public static Result<Flow> Create(string name, string description, FlowStatus status)
    {
        Result<Name> result = Name.Create(name);

        if (result.IsSuccess is false) return Result<Flow>.Failure(result.Message);

        Flow flow = new Flow(result.Value, description, status);
        return Result<Flow>.Success(flow, "Flow criado com sucesso");
    }
}