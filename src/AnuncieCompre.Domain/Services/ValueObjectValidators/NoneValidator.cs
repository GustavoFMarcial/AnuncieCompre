using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidators;

public class NoneValidator : IValueObjectValidator
{
    public Result<ValueObject> Validate(string input)
    {
        return (Result<ValueObject>)Result<ValueObject>.Success("");
    }
}