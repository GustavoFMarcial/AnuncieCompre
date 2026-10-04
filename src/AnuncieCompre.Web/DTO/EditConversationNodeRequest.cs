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

    public List<string> Options { get; set; } = [];

    [Required(ErrorMessage = "Informar se node é final é obrigatório")]
    public bool IsFinal { get; set; }
    [Required(ErrorMessage = "Informar se node é inicial é obrigatório")]
    public bool IsInitial { get; set; }
}