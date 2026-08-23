using System.ComponentModel.DataAnnotations;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Web.DTO;

public record EditConversationFlowStatusRequest
{
    [Required(ErrorMessage = "Status é obrigatório")]
    public string Status { get; set; } = default!;
}