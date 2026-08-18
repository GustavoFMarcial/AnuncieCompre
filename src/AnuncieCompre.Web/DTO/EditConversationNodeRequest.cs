using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record EditConversationNodeRequest
{
    [Required(ErrorMessage = "Mensagem é obrigatória")]
    public string Message { get; set; } = default!;

    [Required(ErrorMessage = "Tipo de validação do node é obrigatório")]
    public string ValidationKind { get; set; } = default!;

    [Required(ErrorMessage = "Validador é obrigatório")]
    public string ValueObjectValidator { get; set; } = default!;

    public string[]? Options { get; set; } = [];

    [Required(ErrorMessage = "É obrigatório informar se node é final")]
    public bool IsFinal { get; set; } = default!;
}