using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Interfaces;

public interface IValueObjectValidator
{
    public Result<ValueObject> Validate(string input);
}