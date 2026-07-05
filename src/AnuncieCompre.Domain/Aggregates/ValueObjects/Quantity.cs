using System.Text.RegularExpressions;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public partial class Quantity : ValueObject
{
    public string Value { get; private set; } = string.Empty;

    private Quantity(){}
    private Quantity(string quantity)
    {
        Value = quantity;
    }

    public static Result<Quantity> Create(string quantity)
    {
        if (!IsValidQuantity(quantity)) return Result<Quantity>.Failure("Quantidade inválida");

        return Result<Quantity>.Success(new Quantity(quantity), "VOQuantity criado com sucesso");
    }

    private static bool IsValidQuantity(string quantity)
    {
        Regex regex = MyRegex();

        return regex.IsMatch(quantity);
    }

    [GeneratedRegex(@"^0*[1-9]\d*.*$")]
    private static partial Regex MyRegex();
}