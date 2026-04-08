using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidator;

public class OptionValidator(string[] options) : IValueObjectValidator
{
    public string[] Options { get; private set; } = options;
    public IResultValueObject Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<Phone>.Failure("Opção não pode ser em branco");

        return Option.Create(Options, input.Trim());
    }
}