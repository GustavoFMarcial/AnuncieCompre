using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record EditConversationNodeTransitionRequest
{
    [Required(ErrorMessage = "Opção é obrigatório")]
    public string Option { get; set; } = default!;

    [Required(ErrorMessage = "Id do próximo Node é obrigatório")]
    public string TargetNodeId { get; set; } = default!;
}