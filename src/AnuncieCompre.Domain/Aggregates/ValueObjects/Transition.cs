using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public class Transition : ValueObject
{
    public string Option { get; private set; } = default!;
    public string TargetNodeId { get; private set; } = default!;

    private Transition(){}

    private Transition(string option, string targetNodeId)
    {
        Option = option;
        TargetNodeId = targetNodeId;
    }

    public static Result<Transition> Create(string option, string targetNodeId)
    {
        if (string.IsNullOrWhiteSpace(option)) return Result<Transition>.Failure("Option não pode ser em branco");
        if (string.IsNullOrWhiteSpace(option)) return Result<Transition>.Failure("TargetNodeId não pode ser em branco");

        return Result<Transition>.Success(new Transition(option, targetNodeId), "Transition criado com sucesso");
    }
}