using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidator;

public class EmailValidator : IValidator
{
    public IResultValueObject Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Result<Email>.Failure("Email não pode ser em branco");

        return Email.Create(input.Trim());
    }
}