using System.Text.RegularExpressions;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public partial class CPF : ValueObject
{
    public string Value { get; private set; } = default!;

    private CPF(){}
    private CPF(string cpf)
    {
        Value = cpf;
    }

    public static Result<CPF> Create(string document)
    {
        if (!CPFIsValid(document)) return Result<CPF>.Failure("CPF inválido");
        
        return Result<CPF>.Success(new CPF(document), "VODocument criado com sucesso");
    }

    private static readonly Regex CpfRegex = MyRegex();

    public static bool CPFIsValid(string cpf)
    {
        if (!CpfRegex.IsMatch(cpf))
            return false;

        string digits = MyRegexCPF1().Replace(cpf, "");

        if (digits.Length != 11)
            return false;

        if (MyRegexCPF2().IsMatch(digits))
            return false;

        return ValidateCPFDigits(digits);
    }

    private static bool ValidateCPFDigits(string digits)
    {
        int[] weights1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] weights2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        static int CalcDigit(string d, int[] weights)
        {
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
                sum += int.Parse(d[i].ToString()) * weights[i];
            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        int digit1 = CalcDigit(digits, weights1);
        if (digit1 != int.Parse(digits[9].ToString()))
            return false;

        int digit2 = CalcDigit(digits, weights2);
        return digit2 == int.Parse(digits[10].ToString());
    }

    [GeneratedRegex(@"\D")]
    private static partial Regex MyRegexCPF1();
    [GeneratedRegex(@"^(\d)\1{10}$")]
    private static partial Regex MyRegexCPF2();
    [GeneratedRegex(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$", RegexOptions.Compiled
    )]
    private static partial Regex MyRegex();
}