using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class QuantityValidator : IValueObjectValidator
{
    public Result<ValueObject> Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<ValueObject>.Failure("Quantidade do produto não pode ser em branco");

        Result<Quantity> result = Quantity.Create(input.Trim());

        if (result.IsSuccess!) return Result<ValueObject>.Failure(result.Message);

        return Result<ValueObject>.Success(result.Value, result.Message);
    }
}