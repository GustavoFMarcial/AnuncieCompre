using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class UserTypeValidator : IValueObjectValidator
{
    public IResultValueObject Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<UserType>.Failure("Tipo de usuário não pode ser em branco");

        return UserType.Create(input.Trim());
    }
}