using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.DTO;

public record ApplicationUserDTO
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Company { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public string? Phone { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}