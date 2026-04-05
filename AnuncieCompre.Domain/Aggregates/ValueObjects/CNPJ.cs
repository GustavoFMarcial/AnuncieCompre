using System.Text.RegularExpressions;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public partial class CNPJ : ValueObject
{
    public string Value { get; private set; } = default!;

    private CNPJ(){}
    private CNPJ(string cnpj)
    {
        Value = cnpj;
    }

    public static Result<CNPJ> Create(string document)
    {
        if (!CNPJIsValid(document)) return Result<CNPJ>.Failure("CNPJ inválido");

        return Result<CNPJ>.Success(new CNPJ(document), "VOCNPJ criado com sucesso");
    }

    private static readonly Regex CnpjRegex = MyRegex();

    public static bool CNPJIsValid(string cnpj)
    {
        if (!CnpjRegex.IsMatch(cnpj))
            return false;

        string digits = MyRegexCNPJ1().Replace(cnpj, "");

        if (digits.Length != 14)
            return false;

        if (MyRegexCNPJ2().IsMatch(digits))
            return false;

        return ValidateCNPJDigits(digits);
    }

    private static bool ValidateCNPJDigits(string digits)
    {
        int[] weights1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] weights2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        static int CalcDigit(string d, int[] weights)
        {
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
                sum += int.Parse(d[i].ToString()) * weights[i];
            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        int digit1 = CalcDigit(digits, weights1);
        if (digit1 != int.Parse(digits[12].ToString()))
            return false;

        int digit2 = CalcDigit(digits, weights2);
        return digit2 == int.Parse(digits[13].ToString());
    }

    [GeneratedRegex(@"\D")]
    private static partial Regex MyRegexCNPJ1();
    [GeneratedRegex(@"^(\d)\1{13}$")]
    private static partial Regex MyRegexCNPJ2();
    [GeneratedRegex(@"^\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2}$", RegexOptions.Compiled
    )]
    private static partial Regex MyRegex();
}