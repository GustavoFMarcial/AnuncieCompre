using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.ValueObjectValidator;

public class ProductValidator : IValueObjectValidator
{
    public IResultValueObject Validate(string input)
    {
        if (string.IsNullOrEmpty(input)) return Result<Product>.Failure("Descrição do produto não pode ser em branco");

        return Product.Create(input.Trim());
    }
}