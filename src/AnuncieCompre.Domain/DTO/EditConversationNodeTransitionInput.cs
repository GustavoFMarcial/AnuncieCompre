using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Domain.DTO;

public record EditConversationNodeTransitionInput
{
    public List<TransitonInput> Transitions { get; set; } = [];
}