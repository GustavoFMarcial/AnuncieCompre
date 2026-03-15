using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class ConversationTempData
{
    public UserType UserType { get; private set;}
    public VOName? Name { get; private set; } = default!;
    public VOEmail? Email { get; private set; } = default!;
    public VOCPF? CPF { get; private set; } = default!;
    public VOCNPJ? CNPJ { get; private set; } = default!;
    public VOProduct? Product { get; private set; } = default!;
    public VOQuantity? Quantity { get; private set; } = default!;
    public CompanyCategory Category { get; private set; }

    public void SetUserType(UserType type) => UserType = type;
    public void SetName(VOName name) => Name = name;
    public void SetEmail(VOEmail email) => Email = email;
    public void SetCPF(VOCPF cpf) => CPF = cpf;
    public void SetCNPJ(VOCNPJ cnpj) => CNPJ = cnpj;
    public void SetProduct(VOProduct product) => Product = product;
    public void SetQuantity(VOQuantity quantity) => Quantity = quantity;
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