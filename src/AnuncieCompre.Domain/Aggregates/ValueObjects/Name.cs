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

    public static Result<Name> Create(string name)
    {
        Result result = ValidateName(name);
        if (!result.IsSuccess) return Result<Name>.Failure(result.Message);

        return Result<Name>.Success(new Name(name), "VOName criado com sucesso");
    }

    private static Result ValidateName(string name)
    {
        if (name.Length < 5) return Result.Failure("Nome deve ter 5 caracteres ou mais");
        if (name.Length > 40) return Result.Failure("Nome deve ter 40 caracteres ou menos");
        if (!MyRegex().IsMatch(name)) return Result.Failure("Nome não poder conter caracteres especiais");

        return Result.Success("Nome validade com sucesso");
    }

    [GeneratedRegex(@"^[\p{L}\s'-]+$")]
    private static partial Regex MyRegex();
}