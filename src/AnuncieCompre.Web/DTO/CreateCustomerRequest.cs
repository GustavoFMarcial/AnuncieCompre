using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record CreateCustomerRequest
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "Telefone é obrigatório")]
    public string Phone { get; set; } = default!;
}