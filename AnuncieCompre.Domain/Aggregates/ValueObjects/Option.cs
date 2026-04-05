using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public class Option : ValueObject
{
    public string Value { get; private set; } = default!;

    private Option(){}
    private Option(string option)
    {
        Value = option;
    }

    public static Result<Option> Create(string[] options, string input)
    {
        foreach (string o in options)
        {
            if (o == input) return Result<Option>.Success(new Option(o), "VOOption criado com sucesso");
        }
        return Result<Option>.Failure("Opção inválida, escolha novamente");        
    }
}