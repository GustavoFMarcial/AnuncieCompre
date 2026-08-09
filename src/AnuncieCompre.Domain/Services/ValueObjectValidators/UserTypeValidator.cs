using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class UserTypeValidator : IValueObjectValidator
{
    public Result<ValueObject> Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<ValueObject>.Failure("Tipo de usuário não pode ser em branco");

        Result<UserType> result = UserType.Create(input.Trim());

        if (result.IsSuccess is false) return Result<ValueObject>.Failure(result.Message);

        return Result<ValueObject>.Success(result.Value, result.Message);
    }
}