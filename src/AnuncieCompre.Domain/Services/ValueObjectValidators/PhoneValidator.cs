using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class PhoneValidator : IValueObjectValidator
{
    public Result<ValueObject> Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<ValueObject>.Failure("Número não pode ser em branco");

        Result<Phone> result = Phone.Create(input.Trim());

        if (result.IsSuccess!) return Result<ValueObject>.Failure(result.Message);

        return Result<ValueObject>.Success(result.Value, result.Message);
    }
}