using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public class VOProduct : ValueObject
{
    public string Description { get; private set; } = default!;

    private VOProduct(){}
    private VOProduct(string description)
    {
        Description = description;
    }

    public static Result<VOProduct> Create(string product)
    {
        if (string.IsNullOrEmpty(product)) return Result<VOProduct>.Failure("Descrição do produto não pode ser em branco");
        if (product.Trim().Length <= 3) return Result<VOProduct>.Failure("Descrição do produto deve ter 4 caracteres ou mais");
        if (product.Trim().Length > 30) return Result<VOProduct>.Failure("Descrição do produto deve ter 30 caracteres ou menos");

        return Result<VOProduct>.Success(new VOProduct(product), "VOProduct criado com sucesso");
    }
}