namespace AnuncieCompre.Web.DTO;

public record CompanyDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Cnpj { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
}