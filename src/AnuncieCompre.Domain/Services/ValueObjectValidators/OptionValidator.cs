using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class OptionValidator(string[] options) : IValueObjectValidator
{
    public string[] Options { get; private set; } = options;
    public Result<ValueObject> Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<ValueObject>.Failure("Opção não pode ser em branco");

        Result<Option> result = Option.Create(Options, input);

        if (result.IsSuccess!) return Result<ValueObject>.Failure(result.Message);

        return Result<ValueObject>.Success(result.Value, result.Message);
    }
}