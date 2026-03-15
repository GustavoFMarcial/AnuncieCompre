using System.Text.RegularExpressions;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public partial class VOName : ValueObject
{
    public string FullName { get; private set; } = default!;

    private VOName(){}
    private VOName(string fullName)
    {
        FullName = fullName;
    }

    public static Result<VOName> Create(string fullName)
    {
        if (string.IsNullOrEmpty(fullName)) return Result<VOName>.Failure("Nome não pode ser em branco");
        if (fullName.Trim().Length <= 15) return Result<VOName>.Failure("Nome deve ter 15 caracteres ou mais");
        if (fullName.Trim().Length > 40) return Result<VOName>.Failure("Nome deve ter 40 caracteres ou menos");
        if (!NameIsValid(fullName)) return Result<VOName>.Failure("Nome não poder conter caracteres especiais");

        return Result<VOName>.Success(new VOName(fullName), "VOName criado com sucesso");
    }

    private static bool NameIsValid(string nome)
    {
        return MyRegex().IsMatch(nome);
    }

    [GeneratedRegex(@"^[\p{L}\s'-]+$")]
    private static partial Regex MyRegex();
}