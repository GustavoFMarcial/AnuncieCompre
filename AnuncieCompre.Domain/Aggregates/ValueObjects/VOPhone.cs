using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public class VOPhone : ValueObject
{
    public string Number { get; private set; } = string.Empty;

    private VOPhone(){}
    private VOPhone(string number)
    {
        Number = number;
    }

    public static Result<VOPhone> Create(string number)
    {
        if (!number.Contains("+55")) return Result<VOPhone>.Failure("Código do país inválido, número de fora do Brasil");

        return Result<VOPhone>.Success(new VOPhone(number), "VOPhone criado com sucesso");
    }
}