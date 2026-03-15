using System.Text.RegularExpressions;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public partial class VOEmail : ValueObject
{
    public string Email { get; private set; } = string.Empty;

    private VOEmail(){}
    private VOEmail(string email)
    {
        Email = email;
    }

    public static Result<VOEmail> Create(string email)
    {
        if (string.IsNullOrEmpty(email)) return Result<VOEmail>.Failure("Email não pode ser em branco");

        if (!EmailIsValid(email)) return Result<VOEmail>.Failure("Email inválido");

        return Result<VOEmail>.Success(new VOEmail(email), "VOEmail criado com sucesso");
    }

    private static bool EmailIsValid(string email)
    {
        return MyRegex().IsMatch(email);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex MyRegex();
}