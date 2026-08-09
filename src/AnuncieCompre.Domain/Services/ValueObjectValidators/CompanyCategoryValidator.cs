using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class CompanyCategoryValidator : IValueObjectValidator
{
    public Result<ValueObject> Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Result<ValueObject>.Failure("Opção não pode ser em branco");

        Result<CompanyCategory> result = CompanyCategory.Create(input.Trim());

        if (result.IsSuccess!) return Result<ValueObject>.Failure(result.Message);

        return Result<ValueObject>.Success(result.Value, result.Message);
    }
}