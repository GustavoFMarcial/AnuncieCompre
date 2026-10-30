using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates;

public class Company : BaseEntity
{
    public Name Name { get; private set; } = default!;
    public List<User> Users { get; private set; } = [];

    private Company(){}
}