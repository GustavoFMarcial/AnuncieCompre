using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class CompanyCategoryValidator : IValueObjectValidator
{
    public IResultValueObject Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Result<CompanyCategory>.Failure("Opção não pode ser em branco");

        return CompanyCategory.Create(input.Trim());
    }
}