using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidator;

public class CnpjValidator : IValidator
{
    public IResultValueObject Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<CNPJ>.Failure("CNPJ inválido");

        return CNPJ.Create(input.Trim());
    }
}