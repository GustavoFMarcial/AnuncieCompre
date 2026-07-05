using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public class Product : ValueObject
{
    public string Value { get; private set; } = default!;

    private Product(){}
    private Product(string description)
    {
        Value = description;
    }

    public static Result<Product> Create(string product)
    {
        if (product.Length <= 3) return Result<Product>.Failure("Descrição do produto deve ter 4 caracteres ou mais");
        if (product.Length > 30) return Result<Product>.Failure("Descrição do produto deve ter 30 caracteres ou menos");

        return Result<Product>.Success(new Product(product), "VOProduct criado com sucesso");
    }
}