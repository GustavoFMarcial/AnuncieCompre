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
    public string Message { get; private set; } = "Mensagem do bot";
    public ValidationKind ValidationKind { get; private set; } = ValidationKind.None;
    public ValueObjectValidator ValueObjectValidator { get; private set; } = ValueObjectValidator.None;
    public Dictionary<string, ConversationNode> Transitions { get; private set; } = [];
    public bool IsInitial { get; private set; } = false;
    public bool IsFinal { get; private set; } = false;
    public bool IsMenu { get; private set; } = false;
    public List<string> Options { get; private set; } = [];

    private ConversationNode() { }

    private ConversationNode(ConversationFlow conversationFlow)
    {
        ConversationFlowId = conversationFlow.Id;
        ConversationFlow = conversationFlow;
    }

    private ConversationNode(ConversationFlow conversationFlow, string message, ValidationKind validationKind, bool isMenu, Dictionary<string, ConversationNode> transitions, List<string> options)
    {
        ConversationFlowId = conversationFlow.Id;
        ConversationFlow = conversationFlow;
        Message = message;
        ValidationKind = validationKind;
        IsMenu = isMenu;
        Transitions = transitions;
        Options = options;
    }

    public static Result<ConversationNode> Create(ConversationFlow conversationFlow)
    {
        return Result<ConversationNode>.Success(new ConversationNode(conversationFlow), "ConversationNode criado com sucesso");
    }

    public static Result<ConversationNode> Create(ConversationFlow conversationFlow, string message, ValidationKind validationKind, bool isMenu, Dictionary<string, ConversationNode> transitions, List<string> options)
    {
        return Result<ConversationNode>.Success(new ConversationNode(conversationFlow, message, validationKind, isMenu, transitions, options), "ConversationNode criado com sucesso");
    }

    public Result Edit(EditConversationNodeInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Message)) return Result<ConversationNode>.Failure("Mensagem não pode ser em branco");
        if (input.ValidationKind is not ValidationKind.Final && input.IsFinal is true) return Result<ConversationNode>.Failure("Apenas node com validação final pode ser marcado como final");
        if (input.ValidationKind is ValidationKind.Validation && input.ValueObjectValidator is ValueObjectValidator.None) return Result<ConversationNode>.Failure("Node de validação deve possuir um validador");
        if (input.ValueObjectValidator is not ValueObjectValidator.None && input.ValidationKind is not ValidationKind.Validation) return Result<ConversationNode>.Failure("Apenas node de validação deve possuir um validador");
        if (input.Options.Count > 0 && input.ValidationKind is ValidationKind.Final) return Result<ConversationNode>.Failure("Apenas nodes de confirmação ou opção podem ter opções");
        if (input.Options.Count > 0 && input.ValidationKind is ValidationKind.Validation) return Result<ConversationNode>.Failure("Apenas nodes de confirmação ou validação podem ter opções");

        Message = input.Message;
        ValidationKind = input.ValidationKind;
        ValueObjectValidator = input.ValueObjectValidator;
        IsInitial = input.IsInitial;
        IsFinal = input.IsFinal;
        Options = input.Options;

        return Result.Success("ConversationNode editado com sucesso");
    }

    public Result EditTransition(List<Transiton> input, List<ConversationNode> nodes)
    {
        Dictionary<string, ConversationNode> transitions = [];

        foreach (Transiton i in input)
        {
            int index = nodes.FindIndex(cn => cn.Id == i.TargetNodeId);

            if (index == -1) return Result.Failure("ConversationNode não pertence ao mesmo ConversationFlow");

            transitions.Add(i.Option, nodes[index]);
        }

        Transitions = transitions;
        return Result.Success("Transições atualizadas com sucesso");
    }

    public void RemoveTransition(Guid targetNodeId)
    {
        KeyValuePair<string, ConversationNode> transition = Transitions.FirstOrDefault(t => t.Value.Id == targetNodeId);
        Transitions.Remove(transition.Key);
    }

    public Result ValidateTransitions(FlowStatus status)
    {
        if (status == FlowStatus.Draft) return Result.Success("Transações validadas com sucesso");
        if (ValidationKind is ValidationKind.Final && Transitions.Count != 1) return Result.Failure("Node final só pode ter uma transição");
        if (ValidationKind is ValidationKind.Validation && Transitions.Count != 1) return Result.Failure("Node de validação só pode ter uma transição");
        if (ValidationKind is ValidationKind.Option && Transitions.Count <= 1) return Result.Failure("Node de opção não pode ter menos de uma transição");
        if (ValidationKind is ValidationKind.Confirmation && Transitions.Count <= 1) return Result.Failure("Node de confirmação não pode ter só uma transição");
        if (Options?.Count != Transitions.Count) return Result.Failure("A quantia de opções deve ser igual a quantia de transições");

        return Result.Success("Transações validadas com sucesso");
    }

    public void SetTransitions(Dictionary<string, ConversationNode> transitions)
    {
        Transitions = transitions;
    }

    public void SetMessage(string message)
    {
        Message = message;
    }

    public void SetOptions(List<string> options)
    {
        Options = options!;
    }
}