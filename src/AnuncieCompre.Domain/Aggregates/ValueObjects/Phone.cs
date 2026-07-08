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
        if (!number.Contains("+55")) return Result<Phone>.Failure("Número de fora do Brasil");
        if (!number.Contains("whatsapp")) return Result<Phone>.Failure("Número não é whatsapp");
        if (number.Length < 22 || number.Length > 23) return Result<Phone>.Failure("Número no formato errado");

        return Result<Phone>.Success(new Phone(number), "VOPhone criado com sucesso");
    }
}