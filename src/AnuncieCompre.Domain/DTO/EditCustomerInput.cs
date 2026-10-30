namespace AnuncieCompre.Domain.DTO;

public record EditCustomerInput
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}