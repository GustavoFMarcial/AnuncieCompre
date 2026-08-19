using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public record NodeTransition : ValueObject
{
    public string Option { get; private set; } = default!;
    public string TargetNodeId { get; private set; } = default!;

    private NodeTransition(){}

    private NodeTransition(string option, string targetNodeId)
    {
        Option = option;
        TargetNodeId = targetNodeId;
    }

    public static Result<NodeTransition> Create(string option, string targetNodeId)
    {
        if (string.IsNullOrWhiteSpace(option)) return Result<NodeTransition>.Failure("Option não pode ser em branco");
        if (string.IsNullOrWhiteSpace(option)) return Result<NodeTransition>.Failure("TargetNodeId não pode ser em branco");

        return Result<NodeTransition>.Success(new NodeTransition(option, targetNodeId), "Transition criado com sucesso");
    }
}