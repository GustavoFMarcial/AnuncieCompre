using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.NodeAggregate;

public class ConversationNode : BaseEntity
{
    public Guid ConversationFlowId { get; private set; }
    public ConversationFlow ConversationFlow { get; private set; } = default!;
    public string Message { get; private set; } = default!;
    public ValidationKind ValidationKind { get; private set; } = ValidationKind.None;
    public ValueObjectValidator ValueObjectValidator { get; private set; }
    public List<NodeTransition> Transitions { get; private set; } = [];
    public bool IsFinal { get; private set; }
    public string[]? Options { get; private set; } = [];

    private ConversationNode(){}

    private ConversationNode(ConversationFlow conversationFlow)
    {
        ConversationFlowId = conversationFlow.Id;
        ConversationFlow = conversationFlow;
    }

    public static Result<ConversationNode> Create(ConversationFlow conversationFlow)
    {
        return Result<ConversationNode>.Success(new ConversationNode(conversationFlow), "ConversationNode criado com sucesso");
    }

    public Result Edit(EditConversationNodeInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Message)) return Result<ConversationNode>.Failure("Mensagem não pode ser em branco");
        if (input.ValidationKind is not ValidationKind.Final && input.IsFinal is true) return Result<ConversationNode>.Failure("Apenas node final pode ser IsFinal");
        if (input.ValidationKind is ValidationKind.Validation && input.ValueObjectValidator is ValueObjectValidator.None) return Result<ConversationNode>.Failure("Node de validação deve possuir um validador");
        if (input.ValueObjectValidator is not ValueObjectValidator.None && input.ValidationKind is not ValidationKind.Validation) return Result<ConversationNode>.Failure("Apenas node de validação deve possuir um validador");
        if (input.Options?.Length > 0 && input.ValidationKind is ValidationKind.Final) return Result<ConversationNode>.Failure("Apenas nodes de confirmação ou opção podem ter opções");
        if (input.Options?.Length > 0 && input.ValidationKind is ValidationKind.Validation) return Result<ConversationNode>.Failure("Apenas nodes de confirmação ou validação podem ter opções");

        return Result.Success("ConversationNode editado com sucesso");
    }

    public Result EditTransition(List<NodeTransition> transitions)
    {
        if (ValidationKind is ValidationKind.Final && transitions.Count != 1) return Result.Failure("Node final só pode ter uma transição");
        if (ValidationKind is ValidationKind.Validation && transitions.Count != 1) return Result.Failure("Node de validação só pode ter uma transição");
        if (ValidationKind is ValidationKind.Option && transitions.Count <= 1) return Result.Failure("Node de opção não pode ter só uma transição");
        if (ValidationKind is ValidationKind.Confirmation && transitions.Count <= 1) return Result.Failure("Node de confirmação não pode ter só uma transição");

        Transitions = transitions;

        return Result.Success("Transições atualizadas com sucesso");
    }
}