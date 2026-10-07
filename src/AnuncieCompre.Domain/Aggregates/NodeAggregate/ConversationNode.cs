using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.TransitionAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.NodeAggregate;

public class ConversationNode : BaseEntity
{
    public Guid ConversationFlowId { get; private set; }
    public ConversationFlow ConversationFlow { get; private set; } = default!;
    public int Number { get; private set; }
    public string Message { get; private set; } = "Mensagem do bot";
    public ValidationKind ValidationKind { get; private set; } = ValidationKind.None;
    public ValueObjectValidator ValueObjectValidator { get; private set; } = ValueObjectValidator.None;
    public List<ConversationNodeTransition> Transitions { get; private set; } = [];
    public bool IsInitial { get; private set; } = false;
    public bool IsFinal { get; private set; } = false;
    public bool IsMenu { get; private set; } = false;
    public List<string> Options { get; private set; } = [];

    private ConversationNode() { }

    private ConversationNode(ConversationFlow conversationFlow, int number)
    {
        ConversationFlowId = conversationFlow.Id;
        ConversationFlow = conversationFlow;
        Number = number;
    }

    private ConversationNode(ConversationFlow conversationFlow, string message, ValidationKind validationKind, bool isMenu, List<ConversationNodeTransition> transitions, List<string> options)
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
        if (conversationFlow.IsMenu is true) return Result<ConversationNode>.Failure("Não é permitido criar ConversationNode com um ConversationFlowMenu");

        int number = conversationFlow.Nodes.Count + 1;
        return Result<ConversationNode>.Success(new ConversationNode(conversationFlow, number), "ConversationNode criado com sucesso");
    }

    public static Result<ConversationNode> Create(ConversationFlow conversationFlow, string message, ValidationKind validationKind, bool isMenu, List<ConversationNodeTransition> transitions, List<string> options)
    {
        return Result<ConversationNode>.Success(new ConversationNode(conversationFlow, message, validationKind, isMenu, transitions, options), "ConversationNode criado com sucesso");
    }

    public Result Edit(EditConversationNodeInput input)
    {
        List<string> errors = [];
        if (IsMenu is true) return Result.Failure("Esse método só pode ser usado por ConversationNodes que não são menu");
        if (string.IsNullOrWhiteSpace(input.Message)) errors.Add($"Node: {Number} - Mensagem não pode ser em branco");
        if (input.ValidationKind is not ValidationKind.Final && input.IsFinal is true) errors.Add($"Node: {Number} - Apenas node com validação final pode ser marcado como final");
        if (input.ValidationKind is ValidationKind.Validation && input.ValueObjectValidator is ValueObjectValidator.None) errors.Add($"Node: {Number} - Node de validação deve possuir um validador");
        if (input.ValueObjectValidator is not ValueObjectValidator.None && input.ValidationKind is not ValidationKind.Validation) errors.Add($"Node: {Number} - Apenas node de validação deve possuir um validador");
        if (input.Options.Count > 0 && input.ValidationKind is ValidationKind.Final) errors.Add($"Node: {Number} - Apenas nodes de confirmação ou opção podem ter opções");
        if (input.Options.Count > 0 && input.ValidationKind is ValidationKind.Validation) errors.Add($"Node: {Number} - Apenas nodes de confirmação ou validação podem ter opções");

        if (errors.Count > 0) return Result.Failure(errors);

        Message = input.Message;
        ValidationKind = input.ValidationKind;
        ValueObjectValidator = input.ValueObjectValidator;
        IsInitial = input.IsInitial;
        IsFinal = input.IsFinal;
        Options = input.Options;

        return Result.Success("ConversationNode editado com sucesso");
    }

    public Result EditTransition(List<ConversationNodeTransition> transitions, List<ConversationNode> nodes)
    {
        if (IsMenu is true) return Result.Failure("Esse método só pode ser usado por ConversationNodes que não são mennu");

        foreach (ConversationNodeTransition t in transitions)
        {
            int index = nodes.FindIndex(cn => cn.Id == t.TargetNodeId);

            if (index == -1) return Result.Failure("ConversationNode não pertence ao mesmo ConversationFlow");
        }

        Transitions = transitions;
        return Result.Success("Transições atualizadas com sucesso");
    }

    public void RemoveTransition(Guid targetNodeId)
    {
        if (IsMenu is true) return;
        ConversationNodeTransition? transition = Transitions.FirstOrDefault(t => t.TargetNodeId == targetNodeId);
        if (transition is null) return;
        Transitions.Remove(transition);
    }

    public Result ValidateTransitions()
    {
        List<string> errors = [];
        if (Transitions.Count <= 0) errors.Add($"Node: {Number} - Todo node deve ter ao menos uma transição");
        if (IsMenu is true) errors.Add($"Node: {Number} - Esse método só pode ser usado por ConversationNodes que não são mennu");
        if (ValidationKind is ValidationKind.Final && Transitions.Count != 1) errors.Add($"Node: {Number} - Node final só pode ter uma transição");
        if (ValidationKind is ValidationKind.Validation && Transitions.Count != 1) errors.Add($"Node: {Number} - Node de validação só pode ter uma transição");
        if (ValidationKind is ValidationKind.Option && Transitions.Count <= 1) errors.Add($"Node: {Number} - Node de opção não pode ter menos de uma transição");
        if (ValidationKind is ValidationKind.Confirmation && Transitions.Count <= 1) errors.Add($"Node: {Number} - Node de confirmação não pode ter só uma transição");
        if (Options?.Count != Transitions.Count) errors.Add($"Node: {Number} - A quantia de opções deve ser igual a quantia de transições");

        if (errors.Count > 0) return Result.Failure(errors);

        return Result.Success("Transações validadas com sucesso");
    }

    public void SetTransitions(List<ConversationNodeTransition> transitions)
    {
        if (IsMenu is false) return;
        Transitions = transitions;
    }

    public void SetMessage(string message)
    {
        if (IsMenu is false) return;
        Message = message;
    }

    public void SetOptions(List<string> options)
    {
        if (IsMenu is false) return;
        Options = options!;
    }

    public static void RearrangeNumber(List<ConversationNode> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Number = i + 1;
        }
    }
}