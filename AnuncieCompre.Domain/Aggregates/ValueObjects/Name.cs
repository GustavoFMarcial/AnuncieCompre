using System.Text.RegularExpressions;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public partial class Name : ValueObject
{
    public string Value { get; private set; } = default!;

    private Name(){}
    private Name(string fullName)
    {
        Value = fullName;
    }

    public static Result<Name> Create(string fullName)
    {
        if (fullName.Length <= 15) return Result<Name>.Failure("Nome deve ter 15 caracteres ou mais");
        if (fullName.Length > 40) return Result<Name>.Failure("Nome deve ter 40 caracteres ou menos");
        if (!NameIsValid(fullName)) return Result<Name>.Failure("Nome não poder conter caracteres especiais");

        return Result<Name>.Success(new Name(fullName), "VOName criado com sucesso");
    }

    private static bool NameIsValid(string nome)
    {
        return MyRegex().IsMatch(nome);
    }

    [GeneratedRegex(@"^[\p{L}\s'-]+$")]
    private static partial Regex MyRegex();
}