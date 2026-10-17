namespace AnuncieCompre.Domain.DTO;

public record EditConversationNodeTransitionInput
{
    public List<Transiton> Transitions { get; set; } = [];
}