using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidator;

public class NameValidator : IValueObjectValidator
{
    public IResultValueObject Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<Name>.Failure("Nome não pode ser em branco");

        return Name.Create(input.Trim());
    }
}