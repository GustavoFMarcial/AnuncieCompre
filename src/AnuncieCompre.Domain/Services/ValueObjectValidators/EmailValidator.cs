using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class EmailValidator : IValueObjectValidator
{
    public Result<ValueObject> Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Result<ValueObject>.Failure("Email não pode ser em branco");

        Result<Email> result = Email.Create(input.Trim());

        if (result.IsSuccess!) return Result<ValueObject>.Failure(result.Message);

        return Result<ValueObject>.Success(result.Value, result.Message);
    }
}