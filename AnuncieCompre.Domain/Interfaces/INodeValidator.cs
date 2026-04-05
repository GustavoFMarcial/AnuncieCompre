using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Interfaces;

public interface IValidator
{
    public IResultValueObject Validate(string input);
}