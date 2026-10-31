namespace AnuncieCompre.Domain.DTO;

public record CreateCompanyInput
{
    public string CompanyName { get; set; } = default!;
    public string Cnpj { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
}