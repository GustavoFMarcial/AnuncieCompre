using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record EditCustomerRequest
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [Length(5, 40, ErrorMessage = "Nome deve ter entre 5 a 40 caracteres")]
    public string Name { get; set; } = default!;
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress]
    public string Email { get; set; } = default!;
}