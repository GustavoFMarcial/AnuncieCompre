using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates;

public class Company : BaseEntity
{
    public Name Name { get; private set; } = default!;
    public CNPJ Cnpj { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Phone Phone { get; private set; } = default!;

    private Company(){}

    private Company(Name name, CNPJ cnpj, Email email, Phone phone)
    {
        Name = name;
        Cnpj = cnpj;
        Email = email;
        Phone = phone;
    }

    public static Result<Company> Create(Name name, CNPJ cnpj, Email email, Phone phone)
    {   
        Company company = new Company(name, cnpj, email, phone);
        return Result<Company>.Success(company, "Company criado com sucesso");
    }
}