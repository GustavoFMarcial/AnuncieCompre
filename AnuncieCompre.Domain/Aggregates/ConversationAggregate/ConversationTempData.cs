using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class ConversationTempData
{
    public UserType UserType { get; private set;}
    public Name? Name { get; private set; } = default!;
    public Email? Email { get; private set; } = default!;
    public CPF? CPF { get; private set; } = default!;
    public CNPJ? CNPJ { get; private set; } = default!;
    public Product? Product { get; private set; } = default!;
    public Quantity? Quantity { get; private set; } = default!;
    public CompanyCategory Category { get; private set; }

    public void SetUserType(UserType type) => UserType = type;
    public void SetName(Name name) => Name = name;
    public void SetEmail(Email email) => Email = email;
    public void SetCPF(CPF cpf) => CPF = cpf;
    public void SetCNPJ(CNPJ cnpj) => CNPJ = cnpj;
    public void SetProduct(Product product) => Product = product;
    public void SetQuantity(Quantity quantity) => Quantity = quantity;
    public void SetCompanyCategory(CompanyCategory category) => Category = category;
    
    public void Clear()
    {
        Name = default!;
        Email = default!;
        Product = default!;
        Quantity = default!;
        CPF = default!;
        CNPJ = default!;
        UserType = default!;
        Category = default!;
    }
}