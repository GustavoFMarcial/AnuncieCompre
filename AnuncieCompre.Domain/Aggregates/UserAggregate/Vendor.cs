using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.UserAggregate;

public class Vendor : BaseEntity
{
    public int UserId { get; private set; }
    public User User { get; private set; } = default!;
    public CNPJ CNPJ { get; private set; } = default!;
    public ValueObjects.CompanyCategory Category { get; private set; } = default!;

    private Vendor(){}

    private Vendor(User user, CNPJ cnpj, ValueObjects.CompanyCategory category)
    {
        UserId = user.Id;
        User = user;
        CNPJ = cnpj;
        Category = category;
    }

    public static Vendor Create(User user, CNPJ cnpj, ValueObjects.CompanyCategory category)
    {
        return new Vendor(user, cnpj, category);
    }
}