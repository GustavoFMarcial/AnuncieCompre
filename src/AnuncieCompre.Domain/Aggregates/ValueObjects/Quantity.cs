using System.Globalization;
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
        var match = MyRegex1().Match(quantity);

        if (!match.Success) return false;

        var number = match.Value.Replace(',', '.');

        if (!decimal.TryParse(number, CultureInfo.InvariantCulture, out var value)) return false;

        return value > 0;
    }

    // [GeneratedRegex(@"^(?!0(?:[.,]0+)?(?:\s|$))\d+(?:[.,]\d+)?(?:\s*.+)?$")]
    // private static partial Regex MyRegex();
    [GeneratedRegex(@"^\d+(?:[.,]\d+)?")]
    private static partial Regex MyRegex1();
}