using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.UserAggregate;

public class User : BaseEntity
{
    // public Guid CompanyId { get; private set; }
    // public Company Company { get; private set; } = default!;
    public Name Name { get; private set; } = default!;
}