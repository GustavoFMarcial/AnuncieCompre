using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public class Phone : ValueObject
{
    public string Value { get; private set; } = string.Empty;

    private Phone(){}
    private Phone(string number)
    {
        Value = number;
    }

    public static Result<Phone> Create(string number)
    {
        if (!number.Contains("+55")) return Result<Phone>.Failure("Código do país inválido, número de fora do Brasil");

        return Result<Phone>.Success(new Phone(number), "VOPhone criado com sucesso");
    }
}