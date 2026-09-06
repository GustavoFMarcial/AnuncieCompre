using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.DTO;

public record EditConversationNodeInput
{
    public string Message { get; set; } = default!;
    public ValidationKind ValidationKind { get; set; }
    public ValueObjectValidator ValueObjectValidator { get; set; }
    public List<string> Options { get; set; } = [];
    public bool IsFinal { get; set; }
    public bool IsInitial { get; set; }
}