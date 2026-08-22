using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record EditConversationFlowRequest
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [Length(5, 40, ErrorMessage = "Nome deve ter entre 5 a 40 caracteres")]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Status é obrigatório")]
    public string Status { get; set; } = default!;
}