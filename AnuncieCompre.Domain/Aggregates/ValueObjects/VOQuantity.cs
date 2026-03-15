using System.Text.RegularExpressions;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public partial class VOQuantity : ValueObject
{
    public string Quantity { get; private set; } = string.Empty;

    private VOQuantity(){}
    private VOQuantity(string quantity)
    {
        Quantity = quantity;
    }

    public static Result<VOQuantity> Create(string quantity)
    {
        if (string.IsNullOrEmpty(quantity)) return Result<VOQuantity>.Failure("Quantidade do produto não pode ser em branco");
        if (!IsValidQuantity(quantity)) return Result<VOQuantity>.Failure("Quantidade inválida");

        return Result<VOQuantity>.Success(new VOQuantity(quantity), "VOQuantity criado com sucesso");
    }

    private static bool IsValidQuantity(string quantity)
    {
        Regex regex = MyRegex();

        return regex.IsMatch(quantity);
    }

    [GeneratedRegex(@"^0*[1-9]\d*.*$")]
    private static partial Regex MyRegex();
}