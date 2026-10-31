using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AnuncieCompre.Infra.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid CompanyId { get; set; }
    public Name Name { get; set; } = default!;
    public CPF Cpf { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}