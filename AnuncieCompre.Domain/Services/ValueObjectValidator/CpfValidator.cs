using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidator;

public class CpfValidator : IValidator
{
    public IResultValueObject Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Result<CPF>.Failure("CPF inválido");

        return CPF.Create(input.Trim());
    }
}