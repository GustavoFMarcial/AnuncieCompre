using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public class CompanyCategory : ValueObject
{
    public Enums.CompanyCategory Value { get; private set; }

    private CompanyCategory(){}
    private CompanyCategory(Enums.CompanyCategory company)
    {
        Value = company;
    }

    public static Result<CompanyCategory> Create(string company)
    {
        string[] companyOptions = CompanyCategoryExtensions.ToStringArray();

        foreach (string c in companyOptions)
        {
            if (c == company) return Result<CompanyCategory>.Success(new CompanyCategory((Enums.CompanyCategory)int.Parse(company)), "CompanyCategory validado com sucesso");
        }
        
        return Result<CompanyCategory>.Failure("Opção inválida, escolha novamente.");
    }
}