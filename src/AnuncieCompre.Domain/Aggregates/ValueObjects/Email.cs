using System.Text.RegularExpressions;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public partial class Email : ValueObject
{
    public string Value { get; private set; } = string.Empty;

    private Email(){}
    private Email(string email)
    {
        Value = email;
    }

    public static Result<Email> Create(string email)
    {
        if (!EmailIsValid(email)) return Result<Email>.Failure("Email inválido");

        return Result<Email>.Success(new Email(email), "VOEmail criado com sucesso");
    }

    private static bool EmailIsValid(string email)
    {
        return MyRegex().IsMatch(email);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex MyRegex();
}