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
    public FlowStatus Status { get; private set; } = default;
    public List<ConversationNode> Nodes { get; private set; } = [];

    private ConversationFlow() { }

    private ConversationFlow(Name name, FlowStatus status, string? description = default)
    {
        Name = name;
        Description = description;
        Status = status;
    }

    public static Result<ConversationFlow> Create(Name name, string? description, FlowStatus status)
    {
        ConversationFlow flow = new(name, status, description);
        return Result<ConversationFlow>.Success(flow, "ConversationFlow criado com sucesso");
    }

    public Result EditFlow(Name name, string? description)
    {
        Name = name;
        Description = description;

        return Result.Success("ConversationFlow editado com sucesso");
    }

    public Result EditStatus(FlowStatus status)
    {
        if (status == FlowStatus.Draft)
        {
            Status = status;
        }
        else
        {
            List<ConversationNode> finalNodes = Nodes.FindAll(n => n.ValidationKind == ValidationKind.Final);
            
            if (finalNodes.Count > 1) return Result.Failure("ConversationFlow deve ter apenas um node marcado como final");
            if (finalNodes.Count < 1) return Result.Failure("ConversationFlow deve ter um node marcado como final");

            Status = status;
        }

        return Result.Success("Status editado com sucesso");
    }
}