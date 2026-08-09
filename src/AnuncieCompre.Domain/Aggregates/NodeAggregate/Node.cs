using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.NodeAggregate;

public class Node : BaseEntity
{
    public string Message { get; private set; } = default!;
    public INodeValidator? NodeValidator { get; private set; }
    public IValueObjectValidator? ValueObjectValidator { get; private set; }
    public string[]? Options { get; private set; } = [];
    public (string options, string targetNodeId)[] Transitions { get; private set; } = [];
    public bool IsFinal { get; private set; }

    private Node(){}

    private Node(string message, (string options, string targetNodeId)[] transitions, bool isFinal, INodeValidator nodeValidator = default!, IValueObjectValidator valueObjectValidator = default!, string[] options = default!)
    {
        Message = message;
        NodeValidator = nodeValidator;
        ValueObjectValidator = valueObjectValidator;
        Options = options;
        Transitions = transitions;
        IsFinal = isFinal;
    }

    public static Result<Node> Create(string message, (string options, string targetNodeId)[] transitions, bool isFinal, INodeValidator nodeValidator = default!, IValueObjectValidator valueObjectValidator = default!, string[] options = default!)
    {
        if (string.IsNullOrWhiteSpace(message)) return Result<Node>.Failure("Mensagem não pode ser em branco");
        if (transitions.Length < 1) return Result<Node>.Failure("Todo node deve ter no mínimo uma transição");
        if (nodeValidator is FinalNodeValidator && transitions.Length != 1) return Result<Node>.Failure("Node final só pode ter uma transição");
        if (nodeValidator is ValidationNodeValidator && transitions.Length != 1) return Result<Node>.Failure("Node de validação só pode ter uma transição");
        if (nodeValidator is OptionNodeValidator && transitions.Length <= 1) return Result<Node>.Failure("Node de opção não pode ter só uma transição");
        if (nodeValidator is ConfirmationNodeValidator && transitions.Length <= 1) return Result<Node>.Failure("Node de confirmação não pode ter só uma transição");
        if (nodeValidator is not FinalNodeValidator && isFinal is true) return Result<Node>.Failure("Apenas node final pode ser IsFinal");
        if (nodeValidator is ValidationNodeValidator && valueObjectValidator is null) return Result<Node>.Failure("Node de validação deve possuir um validador");
        if (valueObjectValidator is not null && nodeValidator is not ValidationNodeValidator) return Result<Node>.Failure("Apenas node de validação deve possuir um validador");
        if (options.Length > 0 && nodeValidator is FinalNodeValidator) return Result<Node>.Failure("Apenas nodes de confirmação ou opção podem ter opções");
        if (options.Length > 0 && nodeValidator is ValidationNodeValidator) return Result<Node>.Failure("Apenas nodes de confirmação ou validação podem ter opções");

        Node node = new Node(message, transitions, isFinal, nodeValidator, valueObjectValidator!, options);
        return Result<Node>.Success(node, "Node criado com sucesso");
    }
}