using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record CreateCustomerInput
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
}