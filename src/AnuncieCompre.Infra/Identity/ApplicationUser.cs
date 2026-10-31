using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AnuncieCompre.Infra.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;
    public Name Name { get; set; } = default!;
    public CPF Cpf { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}