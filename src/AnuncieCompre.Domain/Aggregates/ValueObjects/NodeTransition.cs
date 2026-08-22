using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public record NodeTransition : ValueObject
{
    public string? Option { get; private set; }
    public Guid? TargetNodeId { get; private set; }

    private NodeTransition(){}

    private NodeTransition(string? option, Guid? targetNodeId)
    {
        Option = option;
        TargetNodeId = targetNodeId;
    }

    public static Result<NodeTransition> Create(string? option, Guid? targetNodeId)
    {
        // if (string.IsNullOrWhiteSpace(option)) return Result<NodeTransition>.Failure("Option não pode ser em branco");
        // if (string.IsNullOrWhiteSpace(option)) return Result<NodeTransition>.Failure("TargetNodeId não pode ser em branco");

        return Result<NodeTransition>.Success(new NodeTransition(option, targetNodeId), "Transition criado com sucesso");
    }
}