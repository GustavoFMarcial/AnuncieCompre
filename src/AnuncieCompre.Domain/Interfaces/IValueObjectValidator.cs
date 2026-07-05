using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Interfaces;

public interface IValueObjectValidator
{
    public IResultValueObject Validate(string input);
}