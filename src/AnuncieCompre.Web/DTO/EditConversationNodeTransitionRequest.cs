using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record EditConversationNodeTransitionRequest
{
    [Required(ErrorMessage = "Transições são obrigatórias")]
    public List<TransitonRequest> Transitions { get; set; } = [];
}