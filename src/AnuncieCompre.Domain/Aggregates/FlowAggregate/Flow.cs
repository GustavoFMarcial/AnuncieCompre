using System.Diagnostics;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.FlowAggregate;

public class Flow : BaseEntity
{
    public Name Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public FlowStatus Status { get; private set; } = default!;

    private Flow(Name name, string description, FlowStatus status)
    {
        Name = name;
        Description = description;
        Status = status;
    }

    public static Flow Create(Name name, string description, FlowStatus status)
    {
        return new Flow(name, description, status);
    }
}